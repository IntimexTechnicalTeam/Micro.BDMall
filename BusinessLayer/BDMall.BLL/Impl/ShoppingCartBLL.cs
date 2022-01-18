using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class ShoppingCartBLL : BaseBLL, IShoppingCartBLL
    {
        public IDealProductQtyCacheBLL dealProductQtyCacheBLL;
        public IInventoryBLL InventoryBLL;
        public IProductBLL ProductBLL;

        public ShoppingCartBLL(IServiceProvider services) : base(services)
        {
            dealProductQtyCacheBLL = Services.Resolve<IDealProductQtyCacheBLL>();
            InventoryBLL = Services.Resolve<IInventoryBLL>(); 
            ProductBLL = Services.Resolve<IProductBLL>();
        }

        public List<ShopcartItem> GetShoppingCartItem()
        {
            List<ShopcartItem> data = new List<ShopcartItem>();

            var query = from m in baseRepository.GetList<ShoppingCartItem>()
                               join sku in baseRepository.GetList<ProductSku>() on m.SkuId equals sku.Id into msku
                               from mt in msku.DefaultIfEmpty()
                      
                        where m.MemberId == Guid.Parse(CurrentUser.UserId) && m.IsActive && !m.IsDeleted && m.Product.Status == ProductStatus.OnSale
                        select new
                        {
                            Id = m.Id,
                            SkuId = m.SkuId,
                            AttrValue1 = mt.AttrValue1,
                            AttrValue2 = mt.AttrValue2,
                            AttrValue3 = mt.AttrValue3,
                            AttrId1 = mt.Attr1,
                            AttrId2 = mt.Attr2,
                            AttrId3 = mt.Attr3,

                            ////AttrValue1Price = aap1 == null ? 0 : aap1.AdditionalPrice,
                            ////AttrValue2Price = aap2 == null ? 0 : aap2.AdditionalPrice,
                            ////AttrValue3Price = aap3 == null ? 0 : aap3.AdditionalPrice,
                            
                            //AttrValueName1 = ma == null ? Guid.Empty : ma.DescTransId,
                            //AttrValueName2 = mb == null ? Guid.Empty : mb.DescTransId,
                            //AttrValueName3 = mc == null ? Guid.Empty : mc.DescTransId,

                            //AttrName1 = mat1 == null ? Guid.Empty : mat1.DescTransId,
                            //AttrName2 = mat2 == null ? Guid.Empty : mat2.DescTransId,
                            //AttrName3 = mat3 == null ? Guid.Empty : mat3.DescTransId,
                            Qty = m.Qty,
                            Product = new ProductSummary()
                            {
                                ProductId = m.Product.Id,
                                MarkupPrice = m.Product.MarkUpPrice,
                                SalePrice = m.Product.SalePrice + m.Product.MarkUpPrice,
                                OriginalPrice = m.Product.OriginalPrice + m.Product.MarkUpPrice,
                                Code = m.Product.Code,
                                Name = m.Product.NameTransId.ToString(),
                                IsActive = m.Product.IsActive,
                                CatalogId = m.Product.CatalogId,
                                CurrencyCode = m.Product.CurrencyCode,
                                ApproveType = m.Product.Status,
                                MerchantId = m.Product.MerchantId

                            }
                        };

        //    foreach (var item in query.ToList())
        //    {
        //        ShopcartItem cartItem = new ShopcartItem();
        //        cartItem.CartItemType = ShoppingCartItemType.BUYDONG;

        //        item.Product.Imgs = ProductBLL.GetProductImages(item.Product.ProductId);
        //        item.Product.Currency = CurrencyBLL.GetSimpleCurrency(item.Product.CurrencyCode);// new Model.PaymentMNG.Front.SimpleCurrency();

        //        cartItem.Id = item.Id;
        //        cartItem.SkuId = item.SkuId;
        //        cartItem.Product = item.Product;
        //        cartItem.Qty = item.Qty;

        //        cartItem.AttrValue1 = new ProdAttValue();
        //        cartItem.AttrValue1.Id = item.AttrValue1;
        //        cartItem.AttrValue1.AddPrice = item.AttrValue1Price;
        //        //cartItem.AttrValue1.Name = _translationRepository.GetTranslation(item.AttrValueName1, ReturnDataLanguage)?.Value;

        //        cartItem.AttrValue2 = new ProdAttValue();
        //        cartItem.AttrValue2.Id = item.AttrValue2;
        //        cartItem.AttrValue2.AddPrice = item.AttrValue2Price;
        //        //cartItem.AttrValue2.Name = _translationRepository.GetTranslation(item.AttrValueName2, ReturnDataLanguage)?.Value;

        //        cartItem.AttrValue3 = new ProdAttValue();
        //        cartItem.AttrValue3.Id = item.AttrValue3;
        //        cartItem.AttrValue3.AddPrice = item.AttrValue3Price;
        //        //cartItem.AttrValue3.Name = _translationRepository.GetTranslation(item.AttrValueName3, ReturnDataLanguage)?.Value;


        //        cartItem.Attr1 = new Model.ProductMNG.Front.ProdAtt();
        //        cartItem.Attr1.Id = item.AttrId1;
        //        //cartItem.Attr1.Name = _translationRepository.GetTranslation(item.AttrName1, ReturnDataLanguage)?.Value;

        //        cartItem.Attr2 = new Model.ProductMNG.Front.ProdAtt();
        //        cartItem.Attr2.Id = item.AttrId2;
        //        //cartItem.Attr2.Name = _translationRepository.GetTranslation(item.AttrName2, ReturnDataLanguage)?.Value;

        //        cartItem.Attr3 = new Model.ProductMNG.Front.ProdAtt();
        //        cartItem.Attr3.Id = item.AttrId3;
        //        //cartItem.Attr3.Name = _translationRepository.GetTranslation(item.AttrName3, ReturnDataLanguage)?.Value;

        //        cartItem.AttrList = AttributeBLL.GetInvAttributeByProductWithMapForFront(item.Product.ProductId);

        //        item.Product.Name = translationRepository.GetTranslation(Guid.Parse(item.Product.Name), ReturnDataLanguage)?.Value;
        //        item.Product.Currency = CurrencyBLL.GetSimpleCurrency(item.Product.CurrencyCode);



        //        ////判斷該產品是否有promotion rule并計算rule
        //        //var rule = _promotionRuleRepository.GetProductPromotionRule(item.Product.MerchantId, item.Product.Code);

        //        ////如果規則是買一送一，則在購物車上添加贈送的數量
        //        //if (rule != null)
        //        //{
        //        //    if (rule.PromotionRule == PromotionRuleType.BuySend)
        //        //    {
        //        //        var freeItem = GetFreeProduct(rule, cartItem);
        //        //        if (freeItem != null)
        //        //        {
        //        //            cartItem.Qty += freeItem.Qty;
        //        //            cartItem.FreeQty += freeItem.Qty;
        //        //        }

        //        //    }
        //        //    else if (rule.PromotionRule == PromotionRuleType.GroupSale)
        //        //    {
        //        //        SetGroupSalePrice(rule, cartItem);
        //        //    }
        //        //}

        //        //    data.Add(cartItem);
        //        //}

               return data;
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
            cartItem.Sku = sku.Id;

            var existRecord = baseRepository.GetModel<ShoppingCartItem>(d => d.ProductId == cartItem.ProductId && d.SkuId == sku.Id && d.MemberId == Guid.Parse(CurrentUser.UserId) && d.IsActive && !d.IsDeleted);
            //var rule = _promotionRuleRepository.GetProductPromotionRule(product.MerchantId, cartItem.ProdCode);//贈品都要Hold住

            decimal freeQty = 0;

            if (existRecord == null)
            {
                cartItem.AddQty = cartItem.Qty;//設定增量
                cartItem.ProdCode = product.Code;
                result = await CheckPurchasePermissionAsync(cartItem);
                if (result.Succeeded)
                {
                    UnitOfWork.IsUnitSubmit = true;

                    ShoppingCartItem item = new ShoppingCartItem();
                    item.Id = Guid.NewGuid();
                    item.SkuId = sku.Id;
                    item.MemberId = Guid.Parse(CurrentUser.UserId);
                    item.Qty = cartItem.Qty;
                    item.ProductId = cartItem.ProductId;
                    baseRepository.Insert(item);
                    result = InventoryBLL.InsertInventoryHold(new InventoryHold()
                    {
                        SkuId = sku.Id,
                        MemberId = Guid.Parse(CurrentUser.UserId),
                        Qty = cartItem.Qty + (int)freeQty,
                    });
                    UnitOfWork.Submit();
                }
            }
            else
            {
                cartItem.AddQty = cartItem.Qty;
                cartItem.Qty += existRecord.Qty;
                cartItem.ProdCode = product.Code;
                result = await CheckPurchasePermissionAsync(cartItem);
                if (result.Succeeded)
                {
                    UnitOfWork.IsUnitSubmit = true;

                    existRecord.Qty = cartItem.Qty;
                    baseRepository.Update(existRecord);

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
           
            return result;
        }

        public async Task<SystemResult> UpdateCartItemAsync(Guid itemId, int qty)
        {
            SystemResult result = new SystemResult();

            var existRecord = baseRepository.GetModel<ShoppingCartItem>(d => d.Id == itemId);
            if (existRecord != null)
            {
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

                    UnitOfWork.Submit();

                    result.Succeeded = true;
                    result.Message = BDMall.Resources.Message.UpdateSuccess;

                    if (result.Succeeded && qty > 0)
                    {
                        await this.dealProductQtyCacheBLL.UpdateQtyWhenModifyCart(existRecord.SkuId, qty, oldQty);
                    }
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
                UnitOfWork.IsUnitSubmit = true;

                existRecord.IsDeleted = true;
                baseRepository.Update(existRecord);

                result = InventoryBLL.DeleteInventoryHold(new InventoryHold()
                {
                    SkuId = existRecord.SkuId,
                    MemberId = existRecord.MemberId,
                });

                UnitOfWork.Submit();

                if (result.Succeeded)               
                    await this.dealProductQtyCacheBLL.UpdateQtyWhenDeleteCart(existRecord.SkuId, existRecord.Qty);                
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

            InventoryReserved invSku = new InventoryReserved();
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
    }
}
