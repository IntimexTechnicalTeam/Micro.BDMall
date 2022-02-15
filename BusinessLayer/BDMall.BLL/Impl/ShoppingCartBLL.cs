using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;
using Web.MQ;

namespace BDMall.BLL
{
    public class ShoppingCartBLL : BaseBLL, IShoppingCartBLL
    {
        public IDealProductQtyCacheBLL dealProductQtyCacheBLL;
        public IInventoryBLL InventoryBLL;
        public IProductBLL ProductBLL;
        public IShoppingCartRepository ShoppingCartRepository;
        public IAttributeBLL AttributeBLL;
        public ICurrencyBLL CurrencyBLL;
        public ITranslationRepository TranslationRepository;
        public IPromotionRuleRepository PromotionRuleRepository;
        public IMerchantBLL MerchantBLL;
        public ICodeMasterBLL CodeMasterBLL;

        public ShoppingCartBLL(IServiceProvider services) : base(services)
        {
            dealProductQtyCacheBLL = Services.Resolve<IDealProductQtyCacheBLL>();
            InventoryBLL = Services.Resolve<IInventoryBLL>(); 
            ProductBLL = Services.Resolve<IProductBLL>();
            ShoppingCartRepository = Services.Resolve<IShoppingCartRepository>();
            AttributeBLL= Services.Resolve<IAttributeBLL>();
            CurrencyBLL = Services.Resolve<ICurrencyBLL>();
            TranslationRepository = Services.Resolve<ITranslationRepository>();
            PromotionRuleRepository = Services.Resolve<IPromotionRuleRepository>();
            MerchantBLL = Services.Resolve<IMerchantBLL>();
            CodeMasterBLL = Services.Resolve<ICodeMasterBLL>();
        }

        public ShopCartInfo GetShoppingCart()
        {
            var shoppingCart = new ShopCartInfo();
            shoppingCart.Items = GetShoppingCartItem();

            double taxRate = 0;
            foreach (var item in shoppingCart.Items)
            {
                decimal totalPrice = (item.Qty - item.FreeQty) * (item.Product.SalePrice + item.AttrValue1.AddPrice + item.AttrValue2.AddPrice + item.AttrValue3.AddPrice);
                shoppingCart.ItemsAmount += totalPrice;
                shoppingCart.ItemsTaxAmount += totalPrice * (decimal)taxRate;
                shoppingCart.TotalWeight += item.GrossWeight * item.Qty;
            }

            shoppingCart.ItemsTaxAmount = PriceUtil.SystemPrice(shoppingCart.ItemsTaxAmount);
            shoppingCart.TotalAmount = shoppingCart.ItemsAmount + shoppingCart.DeliveryCharge;
            shoppingCart.Currency = CurrencyBLL.GetSimpleCurrency(CurrentUser.CurrencyCode);
            shoppingCart.Qty = shoppingCart.Items.Sum(d => d.Qty);
            shoppingCart.IsTemp = !CurrentUser.IsLogin;
            
            return shoppingCart;
        }

        public async Task<SystemResult> AddtoCartAsync(CartItem cartItem)
        {
            SystemResult result = new SystemResult();

            var product = baseRepository.GetModel<Product>(x => x.Id == cartItem.ProductId);
            if (product == null)
            {
                result.Succeeded = false;
                result.Message = Resources.Message.ProductNotExist;
                return result;
            }

            //判断是否在发售时间
            if (!string.IsNullOrEmpty(product.Remark))
            {
                var timeArr = product.Remark.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (timeArr.Length >= 2)
                {
                    var time = DateUtil.ConvertoDateTime(timeArr[1], "yyyy-MM-dd HH:mm:00");
                    if (DateTime.Now < time)
                    {
                        result.Succeeded = false;
                        //result.Message = Resources.Message.NotSelling;
                        return result;
                    }
                }
            }

            if (cartItem.ProdCode.IsEmpty()) cartItem.ProdCode = product.Code;

            var sku = baseRepository.GetModel<ProductSku>(d => d.ProductCode == cartItem.ProdCode && d.IsActive && !d.IsDeleted && d.ProductCode == cartItem.ProdCode && d.AttrValue1 == cartItem.Attr1 && d.AttrValue2 == cartItem.Attr2 && d.AttrValue3 == cartItem.Attr3 && d.IsActive && !d.IsDeleted);
            if (sku == null) throw new BLException("sku is empty");
            
            cartItem.Sku = sku.Id;

            var saleOut = (await ProductBLL.GetSelloutSkus()).Any(x=>x == sku.Id.ToString());
            if (saleOut) throw new BLException("AddToCartFail: Sale Out");

            ShoppingCartItem item = baseRepository.GetModel<ShoppingCartItem>(d => d.ProductId == cartItem.ProductId && d.SkuId == sku.Id && d.MemberId == Guid.Parse(CurrentUser.UserId) && d.IsActive && !d.IsDeleted);
            //var rule = _promotionRuleRepository.GetProductPromotionRule(product.MerchantId, cartItem.ProdCode);//贈品都要Hold住

            decimal freeQty = 0;
            bool flag = false;

            var timeOut = CodeMasterBLL.GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Order.ToString(), "ShopcartTimeout")?.Value ?? "30";
            if (item == null)
            {
                cartItem.AddQty = cartItem.Qty;//設定增量
                cartItem.ProdCode = product.Code;
                result = await CheckPurchasePermissionAsync(cartItem);
                if (result.Succeeded)
                {
                    UnitOfWork.IsUnitSubmit = true;
                    item = new ShoppingCartItem();
                    item.Id = Guid.NewGuid();
                    item.SkuId = sku.Id;
                    item.MemberId = Guid.Parse(CurrentUser.UserId);
                    item.Qty = cartItem.Qty;
                    item.ProductId = cartItem.ProductId;
                    item.ExpireDate = item.CreateDate.AddMinutes(timeOut.ToInt());
                    baseRepository.Insert(item);
                    CreateShoppingCartItemDetail(item);

                    result = InventoryBLL.InsertInventoryHold(new InventoryHold()
                    {
                        SkuId = sku.Id,
                        MemberId = Guid.Parse(CurrentUser.UserId),
                        Qty = cartItem.Qty + (int)freeQty,
                    });
                    UnitOfWork.Submit();
                    flag = true;
                }
            }
            else
            {
                cartItem.AddQty = cartItem.Qty;
                cartItem.Qty += item.Qty;
                cartItem.ProdCode = product.Code;
 
                result = await CheckPurchasePermissionAsync(cartItem);
                if (result.Succeeded)
                {
                    UnitOfWork.IsUnitSubmit = true;

                    item.Qty = cartItem.Qty;
                    baseRepository.Update(item);
                    CreateShoppingCartItemDetail(item);

                    result = InventoryBLL.InsertInventoryHold(new InventoryHold()
                    {
                        SkuId = sku.Id,
                        MemberId = Guid.Parse(CurrentUser.UserId),
                        Qty = cartItem.Qty + (int)freeQty,
                    });
                    UnitOfWork.Submit();
                }
            }

            if (result.Succeeded)
            {
                await dealProductQtyCacheBLL.UpdateQtyWhenAddToCart(sku.Id, cartItem.AddQty + (int)freeQty);
                result.Message = BDMall.Resources.Message.AddtoCartSuccess;
            }

            ////发送延时队列
            if (flag)
            {
                var millionSecond = timeOut.ToInt() * 60 * 1000;     //分钟转毫秒
                rabbitMQService.PublishDelayMsg(MQSetting.DelayShoppingCartTimeOutQueue,
                    MQSetting.WeChatShoppingCartTimeOutQueue, MQSetting.WeChatShoppingCartTimeOutExchange, item.Id.ToString(), millionSecond);
            }

            return result;
        }

        public async Task<SystemResult> UpdateCartItemAsync(Guid itemId, int qty)
        {
            SystemResult result = new SystemResult();

            var existRecord = baseRepository.GetModel<ShoppingCartItem>(d => d.Id == itemId);
            if (existRecord == null) throw new BLException("itemid is empty");


            int oldQty = existRecord.Qty;
            CartItem cartItem = new CartItem();
            cartItem.ProductId = existRecord.ProductId;
            cartItem.Sku = existRecord.SkuId;
            cartItem.Qty = qty;
            cartItem.AddQty = qty - existRecord.Qty;//計算增量
            existRecord.Qty = qty;
            result = await CheckPurchasePermissionAsync(cartItem);
            if (result.Succeeded)
            {
                UnitOfWork.IsUnitSubmit = true;

                baseRepository.Update(existRecord);

                if (qty > 0)
                {
                    var holdRes = InventoryBLL.InsertInventoryHold(new InventoryHold()
                    {
                        SkuId = cartItem.Sku,
                        MemberId = Guid.Parse(CurrentUser.UserId),
                        Qty = cartItem.Qty,
                    });
                }
                else
                {
                    var holdRes = InventoryBLL.DeleteInventoryHold(new InventoryHold()
                    {
                        SkuId = existRecord.SkuId,
                        MemberId = existRecord.MemberId,
                    });
                }

                CreateShoppingCartItemDetail(existRecord);

                UnitOfWork.Submit();

                result.Succeeded = true;
                result.Message = BDMall.Resources.Message.UpdateSuccess;

                if (result.Succeeded && qty > 0)
                {
                    await this.dealProductQtyCacheBLL.UpdateQtyWhenModifyCart(existRecord.SkuId, qty, oldQty);
                }
            }

            return result;
        }

        public async Task<SystemResult> RemoveFromCart(Guid itemId)
        {
            SystemResult result = new SystemResult();

            var existRecord = baseRepository.GetModel<ShoppingCartItem>(d => d.Id == itemId);
            if (existRecord != null)
            {
                var items = baseRepository.GetList<ShoppingCartItemDetail>(x => x.ShoppingCartItemId == itemId).ToList();
                result = await RemoveFromCart(existRecord);
            }
            return result;
        }

        public async Task<SystemResult> ClearShoppingCart()
        {
            var result = new SystemResult();
            var options = baseRepository.GetList<ShoppingCartItem>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && x.IsActive && !x.IsDeleted).ToList();

            UnitOfWork.IsUnitSubmit = true;
            foreach (var item in options)
            {
                item.IsDeleted = true;

                result = InventoryBLL.DeleteInventoryHold(new InventoryHold()
                {
                    SkuId = item.SkuId,
                    MemberId = item.MemberId,
                });
                baseRepository.Update(item);
                await this.dealProductQtyCacheBLL.UpdateQtyWhenDeleteCart(item.SkuId, item.Qty);
            }
            UnitOfWork.Submit();
            return result;  
        }

        /// <summary>
        /// 获取当前用户是否有产品的购买权限
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<SystemResult> CheckPurchasePermissionAsync(CartItem item)
        {
            var result = new SystemResult() { Succeeded = true };

            var productExt = baseRepository.GetModel<ProductExtension>(x => x.Id == item.ProductId);

            #region 判斷購買數量限制
            var qtyLimit = new { Min = productExt.MinPurQty, Max = productExt.MaxPurQty };
            if (qtyLimit.Min == 0 || qtyLimit.Max == 0)
            {
                //不限制
                return result;
            }

            //限制購買 
            if (item.Qty < qtyLimit.Min)
            {
                result.Succeeded = false;
                //result.Message = Resources.Message.LessThenMin;
                return result;
            }

            //限制購買   
            if (item.Qty > qtyLimit.Max)
            {
                result.Succeeded = false;
                //result.Message = Resources.Message.MoreThenMin;             
                return result;
            }

            if (item.Qty > BDMall.Runtime.Setting.PurchaseLimit) //最大購買量
            {
                result.Succeeded = false;
                //result.Message = Resources.Message.MoreThenMin;                 
                return result;
            }

            #endregion

            #region 判斷是否有庫存

            var sellOutSkuList = await ProductBLL.GetSelloutSkus();
            if (sellOutSkuList != null && sellOutSkuList.Any(d => d == item.Sku.ToString()))
            {
                result.Succeeded = false;
                result.Message = OrderErrorEnum.Sellout.ToString();
                return result;
            }

            var invSku = new InventoryReservedDto();
            invSku.Sku = item.Sku;

            var totalAvailable = InventoryBLL.GetTotAvailableInvQty(invSku);

            if (item.AddQty > 0)//增加產品數量時才判斷庫存，如果小於0表示系釋放產品不需要判斷是否有庫存
            {
                if (totalAvailable <= 0)//可銷售數量為0 
                {                   
                    result.Succeeded = false;                  
                    result.Message = OrderErrorEnum.Sellout.ToString();
                    return result;
                }
                else
                {
                    //少於購買增量，不能購買
                    if (totalAvailable < item.AddQty)
                    {
                        result.Succeeded = false;                      
                        result.Message = OrderErrorEnum.OutOfStock.ToString();
                        return result;
                    }               
                }
            }
            #endregion

            return result;
        }
        
        /// <summary>
        /// 创建购物车明细数据
        /// </summary>
        /// <param name="item"></param>
        private void CreateShoppingCartItemDetail(ShoppingCartItem item)
        { 
            var detail = ShoppingCartRepository.GetItemDetail(item);
            var deleteDetails = baseRepository.GetList<ShoppingCartItemDetail>(x=>x.ShoppingCartItemId== item.Id).ToList();

            var dbDetails = AutoMapperExt.MapTo<ShoppingCartItemDetail>(detail);

            dbDetails.Id = Guid.NewGuid();
            dbDetails.CreateDate = DateTime.Now;
            dbDetails.IsActive = true;
            dbDetails.IsDeleted = false;
            dbDetails.ShoppingCartItemId = item.Id;
            dbDetails.Qty = item.Qty;
            dbDetails.MemberId = item.MemberId;         
            dbDetails.SkuId = item.SkuId;
            dbDetails.ProductId = item.ProductId; 

            baseRepository.Delete(deleteDetails);
            baseRepository.Insert(dbDetails);           
        }

        private List<ShopcartItem> GetShoppingCartItem()
        {           
            var query = baseRepository.GetList<ShoppingCartItemDetail>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && x.IsActive && !x.IsDeleted)
                            .Select(item => new ShopcartItem
                            {
                                Id = item.Id,
                                SkuId = item.SkuId,
                                CartItemType = ShoppingCartItemType.BUYDONG,
                                AttrValue1 = new ProdAttValue { Id = item.AttrValue1, AddPrice = item.AttrValue1Price },
                                AttrValue2 = new ProdAttValue { Id = item.AttrValue2, AddPrice = item.AttrValue2Price },
                                AttrValue3 = new ProdAttValue { Id = item.AttrValue3, AddPrice = item.AttrValue3Price },
                                Attr1 = new ProdAtt { Id = item.AttrId1 },
                                Attr2 = new ProdAtt { Id = item.AttrId2 },
                                Attr3 = new ProdAtt { Id = item.AttrId3 },
                                Qty = item.Qty,
                                Product = new ProductSummary()
                                {
                                    ProductId = item.ProductId,
                                    Code = item.ProductCode,
                                    MerchantId = item.MerchantId,                                   
                                }
                            }).ToList();

            foreach (var item in query)
            {
                var product = baseRepository.GetModelById<Product>(item.Product.ProductId);

                item.AttrList = AttributeBLL.GetInvAttributeByProductWithMapForFront(item.Product.ProductId);
                item.Product.MerchantName = MerchantBLL.GetMerchById(item.Product.MerchantId).Name ?? "";
                item.Product.Name = TranslationRepository.GetDescForLang(product.NameTransId, CurrentUser.Lang);
                item.Product.Currency = CurrencyBLL.GetSimpleCurrency(product.CurrencyCode);
                item.Product.Imgs = ProductBLL.GetProductImages(item.Product.ProductId);
                item.Product.MarkupPrice = product.MarkUpPrice;
                item.Product.SalePrice = product.SalePrice + product.MarkUpPrice;
                item.Product.OriginalPrice = product.OriginalPrice + product.MarkUpPrice;
                item.Product.CatalogId = product.CatalogId;
                item.Product.ApproveType = product.Status;
                item.Product.CreateDate = product.CreateDate;
                item.Product.UpdateDate = product.UpdateDate;

                GenPromotionRule(item);
            }

          
            return query;
        }

        private void GenPromotionRule(ShopcartItem cartItem)
        {
            //判斷該產品是否有promotion rule并計算rule
            var rule = PromotionRuleRepository.GetProductPromotionRule(cartItem.Product.MerchantId, cartItem.Product.Code);

            //如果規則是買一送一，則在購物車上添加贈送的數量
            if (rule != null)
            {
                if (rule.PromotionRule == PromotionRuleType.BuySend)
                {
                    var freeItem = GetFreeProduct(rule, cartItem);
                    if (freeItem != null)
                    {
                        cartItem.Qty += freeItem.Qty;
                        cartItem.FreeQty += freeItem.Qty;
                    }
                }
                else if (rule.PromotionRule == PromotionRuleType.GroupSale)
                {
                    SetGroupSalePrice(rule, cartItem);
                }
            }
        }

        public ShopcartItem GetFreeProduct(PromotionRuleView rule, ShopcartItem cartItem)
        {
            ShopcartItem freeItem = new ShopcartItem();
            decimal setQty = Math.Floor(cartItem.Qty / rule.X);

            if (setQty >= 1)
            {
                ClassUtility.CopyValue(freeItem, cartItem);
                freeItem.Qty = (int)setQty * (int)rule.Y;
                freeItem.IsFree = true;
                freeItem.RuleId = Guid.Empty;
                ProductSummary product = new ProductSummary();
                ClassUtility.CopyValue(product, cartItem.Product);
                product.SalePrice = 0;
                product.SalePrice2 = 0;
                //product.IconRType = ProductType.FreeR;
                product.IconRUrl = PathUtil.GetProductIconUrl(product.IconRType,CurrentUser.Lang);
                //product.IconLType = ProductType.FreeL;
                product.IconLUrl = PathUtil.GetProductIconUrl(product.IconLType,CurrentUser.Lang);
                freeItem.Product = product;
                return freeItem;
            }
            else
            {
                return null;
            }
        }

        public void SetGroupSalePrice(PromotionRuleView rule, ShopcartItem cartItem)
        {

            decimal setQty = Math.Floor(cartItem.Qty / rule.X);

            if (setQty >= 1)
            {
                cartItem.RuleId = rule.Id;
                cartItem.RuleType = PromotionRuleType.GroupSale;
                cartItem.GroupSaleDiscountPrice = Math.Round((setQty * rule.X * cartItem.Product.SalePrice) - (setQty * rule.Y), 2);

                //var a = (setQty * rule.Y) + ((cartItem.Qty - (setQty * rule.X)) * cartItem.Product.SalePrice);
                cartItem.SingleDiscountPrice = Math.Round(cartItem.GroupSaleDiscountPrice / cartItem.Qty, 2);
            }

        }

        /// <summary>
        /// 处理购物车过期
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<SystemResult> RemoveFromCart(ShoppingCartItem shoppingCart)
        {
            var items = baseRepository.GetList<ShoppingCartItemDetail>(x => x.ShoppingCartItemId == shoppingCart.Id).ToList();

            UnitOfWork.IsUnitSubmit = true;

            shoppingCart.IsDeleted = true;
            shoppingCart.UpdateDate = DateTime.Now;
            baseRepository.Update(shoppingCart);
            baseRepository.Delete(items);                   //直接硬删

            var result = InventoryBLL.DeleteInventoryHold(new InventoryHold()
            {
                SkuId = shoppingCart.SkuId,
                MemberId = shoppingCart.MemberId,
            });

            UnitOfWork.Submit();

            if (result.Succeeded)
                await this.dealProductQtyCacheBLL.UpdateQtyWhenDeleteCart(shoppingCart.SkuId, shoppingCart.Qty);

            return result;
        }
    }
}
