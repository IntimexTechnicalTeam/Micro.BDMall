using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Utility;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class OrderBLL : BaseBLL, IOrderBLL
    {
        public IOrderRepository orderRepository;
        public ICodeMasterRepository codeMasterRepository;
        public ICurrencyBLL currencyBLL;
        public IPaymentBLL paymentBLL;
        public IProductBLL productBLL;
        public ITranslationRepository translationRepository;
        public IProductRepository productRepository;
        public IDeliveryAddressBLL deliveryAddressBLL;
        public ICouponRepository couponRepository;
        public ISettingBLL settingBLL;
        public IMerchantBLL merchantBLL;
        public IDealProductQtyCacheBLL dealProductQtyCacheBLL;
        public IInventoryBLL inventoryBLL;
        public IOrderDeliveryRepository orderDeliveryRepository;
        public IDeliveryBLL deliveryBLL;
       

        public OrderBLL(IServiceProvider services) : base(services)
        {
            orderRepository = Services.Resolve<IOrderRepository>();
            codeMasterRepository = Services.Resolve<ICodeMasterRepository>();
            currencyBLL = Services.Resolve<ICurrencyBLL>();
            paymentBLL= Services.Resolve<IPaymentBLL>();
            productBLL = Services.Resolve<IProductBLL>();   
            translationRepository= Services.Resolve<ITranslationRepository>();
            productRepository = Services.Resolve<IProductRepository>();
            deliveryAddressBLL = Services.Resolve<IDeliveryAddressBLL>();
            couponRepository = Services.Resolve<ICouponRepository>();
            settingBLL= Services.Resolve<ISettingBLL>();
            merchantBLL= Services.Resolve<IMerchantBLL>();
            dealProductQtyCacheBLL = Services.Resolve<IDealProductQtyCacheBLL>();
            inventoryBLL = Services.Resolve<IInventoryBLL>();
            orderDeliveryRepository = Services.Resolve<IOrderDeliveryRepository>();
            deliveryBLL = Services.Resolve<IDeliveryBLL>();
            
        }

        public PageData<OrderSummaryView> GetSimpleOrders(OrderCondition cond)
        {
            PageData<OrderSummaryView> result = new PageData<OrderSummaryView>();
            var orders = orderRepository.GetSimpleOrderByPage(cond);
            result.Data = orders.Data.Select(d => GenOrderSummaryView(d)).ToList();     
            result.TotalRecord = orders.TotalRecord;
            return result;
        }

        public OrderInfoView GetOrder(Guid orderId)
        {
            return BuildModel(baseRepository.GetModelById<Order>(orderId));
        }

        public async Task<SystemResult> BuildOrder(NewOrder checkout)
        {
            if (checkout != null)
            {
                foreach (var item in checkout.Items)
                {
                    if (item.DeliveryType == DeliveryType.D)
                    {
                        if (item.AddressId == Guid.Empty) throw new BLException(Resources.Message.DeliveryAddressRequire);
                    }

                    if (item.DeliveryType == null) throw new BLException(Resources.Message.DeliveryTypeRequire);
                    if (item.DeliveryType == DeliveryType.D && item.ChargeId == Guid.Empty) throw new BLException(Resources.Message.CourierRequire);
                    if (item.ChargeInfo == null) throw new BLException("express ChargeInfo is null");

                    string md5Formate = "{0}{1}{2}{3}";
                    var express = item.ChargeInfo;
                    md5Formate = string.Format(md5Formate, express.Id, express.Price.ToString("N4"), express.Discount.ToString("N4"), StoreConst.DeliveryPriceSalt);
                    var vcode = HashUtil.MD5(md5Formate);
                    if (vcode != item.ChargeInfo.Vcode) throw new BLException(BDMall.Resources.Message.SelectAddressAgain);

                    item.Freight = item.ChargeInfo.DiscountPrice;
                    item.OriginalFreight = item.ChargeInfo.DiscountOriginalPrice;
                    item.FreightDiscount = item.ChargeInfo.Discount;
                    item.ServiceType = item.ChargeInfo.ServiceType;
                    item.CountryCode = item.ChargeInfo.CountryCode;
                    item.ExpressCompanyId = item.ChargeInfo.ExpressCompanyId;
                }
                if (checkout.PaymentMethodId == Guid.Empty) throw new BLException(Resources.Message.PaymentTypeRequire);
            }

            var result = CreateOrder(checkout);
            string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
            await RedisHelper.DelAsync(key);
            return result;
        }

        public async Task<SystemResult> UpdateOrderStatus(UpdateStatusCondition cond)
        {
            if (!CheckDeliveryStautsIsSame(cond)) throw new Exception(Resources.Message.StatusHasChanged);

            var result = new SystemResult();

            var dbOrder = baseRepository.GetModelById<Order>(cond.OrderId);
            if (dbOrder != null)
            {
                var order = AutoMapperExt.MapTo<OrderDto>(dbOrder);
                var deliverys = baseRepository.GetList<OrderDelivery>(x=>x.OrderId == order.Id && x.IsActive && !x.IsDeleted).ToList();
                order.OrderDeliverys = AutoMapperExt.MapTo<List<OrderDeliveryDto>>(deliverys);
                order.Currency = new SimpleCurrency() { Code = order.CurrencyCode, Name = order.CurrencyCode };
                switch (cond.Status)
                {
                    case OrderStatus.PaymentConfirmed:
                        UpdateOrderStatusToPayConfirm(order, cond);
                        break;
                    case OrderStatus.Processing:
                        UpdateOrderStatusToProcess(order, cond);
                        break;
                    case OrderStatus.DeliveryArranged:
                        UpdateOrderStatusToDeliveryArranged(order, cond);
                        break;
                    case OrderStatus.OrderCompleted:
                        UpdateOrderStatusToOrderCompleted(order, cond);
                        break;
                    case OrderStatus.SCancelled:
                        UpdateOrderStatusToCancel(order, cond);
                        break;
                }

                result.Succeeded = true;
                if (result.Succeeded) result= await dealProductQtyCacheBLL.UpdateQtyWhenOrderStateChange(cond);
            }

            return result;
        }

        /// <summary>
        /// 更新订单为已确认付款
        /// </summary>
        /// <param name="order"></param>
        /// <param name="cond"></param>
        public void UpdateOrderStatusToPayConfirm(OrderDto order, UpdateStatusCondition cond)
        {

            if (CheckDeliveryStautsIsSame(cond))
            {
                UpdateStatus(order, OrderStatus.PaymentConfirmed, cond);

                if (order.Status == OrderStatus.PaymentConfirmed)
                {
                    UpdateProductSaleSummary(order);//統計產品銷量

                    //SaveDebug(GetType().FullName, ClassUtility.GetMethodName(), "添加预留", "OrderNo:" + order.OrderNO);
                    //添加庫存預留記錄
                    var skuIdList = new List<Guid>();
                    order.OrderDeliverys.ToList();

                    AddInventoryReserved(order, out skuIdList);

                    //SaveDebug(GetType().FullName, ClassUtility.GetMethodName(), "添加预留结束", "OrderNo:" + order.OrderNO);
                    //低庫存/零庫存檢查並發出通知郵件
                    //Task.Run(async () =>
                    //{

                    //    var builder = new ContainerBuilder();
                    //    BLL.Core.AutofacRegister.Reg(builder);
                    //    var container = builder.Build();
                    //    IInventoryChangeNotifyBLL inventoryChangeNotifyBLL;
                    //    using (var scope = container.BeginLifetimeScope())
                    //    {
                    //        inventoryChangeNotifyBLL = scope.Resolve<IInventoryChangeNotifyBLL>();
                    //    }

                    //    await inventoryChangeNotifyBLL.CheckAndNotifyAsync(skuIdList);

                    //});
                }
            }
        }

        /// <summary>
        /// 更新订单状态为已按排送货
        /// </summary>
        /// <param name="order"></param>
        /// <param name="cond"></param>
        public void UpdateOrderStatusToDeliveryArranged(OrderDto order, UpdateStatusCondition cond)
        {
            UpdateStatus(order, OrderStatus.DeliveryArranged, cond);
            UpdateInventoryQty(order, cond);
        }

         /// <summary>
         /// 更新订单状态为已完成
         /// </summary>
         /// <param name="order"></param>
         /// <param name="cond"></param>
        public void UpdateOrderStatusToOrderCompleted(OrderDto order, UpdateStatusCondition cond)
        {

            UpdateStatus(order, OrderStatus.OrderCompleted, cond);

            if (order.Status == OrderStatus.OrderCompleted)
            {
                //更新商家銷售數量
                UpdateMerchantSalesStatistic(order);
                //更新产品的销售数量
                UpdateProductPurchaseStatistic(order);
            }
        }

        public void UpdateOrderStatusToCancel(OrderDto order, UpdateStatusCondition cond)
        {
            UpdateStatus(order, OrderStatus.SCancelled, cond);
            if (order.Status == OrderStatus.SCancelled)
            {
                CancelInventoryQty(order, cond);
            }
        }

        public void UpdateOrderStatusToECancel(OrderDto order, UpdateStatusCondition cond)
        {
            UpdateStatus(order, OrderStatus.ECancelled, cond);
            if (order.Status == OrderStatus.ECancelled)
            {
                CancelInventoryQty(order, cond);
            }
        }

        private void UpdateInventoryQty(OrderDto order, UpdateStatusCondition cond) {

            UnitOfWork.IsUnitSubmit = true;
            //var order = _orderRepository.Entities.FirstOrDefault(p => p.Id == orderId);
            //var orderDetails = _orderDetailRepository.GetByOrderId(orderId);

            var orderDeliveryInfo = cond.DeliveryTrackingInfo.FirstOrDefault();

            var orderDeliveries = order.OrderDeliverys;

            var orderDelivery = orderDeliveries.FirstOrDefault(p => p.Id == orderDeliveryInfo.Id);

            if (orderDelivery.SendBackCount == 0)//没有经过退回的送货单才能更新预留数量
            {
                //foreach (var item in orderDeliveries)
                //{
                //    var deliveryInfo = cond.DeliveryTrackingInfo.FirstOrDefault(p => p.Id == item.Id);

                if (orderDelivery.Status == OrderStatus.DeliveryArranged)
                {
                    var deliveryItems = orderDelivery.DeliveryDetails.GroupBy(g => g.SkuId).Select(d => new
                    {
                        SkuId = d.Key,
                        Qty = d.Sum(a => a.Qty)
                    }).ToList();

                    foreach (var product in deliveryItems)
                    {
                        var reserve = new InventoryReservedDto();
                        reserve.Sku = product.SkuId;
                        reserve.SubOrderId = orderDelivery.Id;
                        reserve.WHId = orderDeliveryInfo.LocationId;
                        reserve.OrderId = order.Id;
                        //reserve.ReservedQty = product.Qty;

                        var result = inventoryBLL.DeductInvQtyWithReserve(reserve);
                        if (!result.Succeeded)
                        {
                            //var whName = inventoryBLL.GetWarehouseById(orderDeliveryInfo.LocationId)?.Name ?? string.Empty;
                            //string msg = Resources.Label.Warehouse + "【" + whName + "】";
                            //SystemError sysError = (SystemError)result.ReturnValue;
                            //if (sysError.Code == (int)InventoryErrorEnum.RecordNotExsit)
                            //{
                            //    msg += Resources.Message.HaveNoInventoryRecord;
                            //    throw new BLException(msg);
                            //}
                            //else if (sysError.Code == (int)InventoryErrorEnum.InventoryQtyNotEnough)
                            //{
                            //    msg += Resources.Message.InvenToryQtyNotEnough;
                            //    throw new BLException(msg);
                            //}
                            //else
                            //{
                            //    throw new BLException(result.Message);
                            //}
                        }
                        else
                        {
                            var returnObj = result.ReturnValue as InventoryReservedDto;
                            if (returnObj != null && returnObj.WHId != null)
                            {
                                var orderDeliveryDetailList = orderDelivery.DeliveryDetails.Where(x => x.SkuId == product.SkuId).ToList();
                                if (orderDeliveryDetailList?.Count > 0)
                                {
                                    //檢查原發貨倉庫是否與實際返貨倉庫一致，不一致則需要更新
                                    if (returnObj.WHId != orderDeliveryDetailList.FirstOrDefault()?.LocationId)
                                    {
                                        foreach (var detail in orderDeliveryDetailList)
                                        {
                                            var dbDetail = baseRepository.GetModel<OrderDeliveryDetail>(x=>x.Id == detail.Id);
                                            dbDetail.LocationId = returnObj.WHId.Value;
                                            baseRepository.Update(dbDetail);
                                        }
                                    }
                                }
                            }
                        }
                    }                   
                }
            }
            UnitOfWork.Submit();
        }


        /// <summary>
        /// 取消訂單時取消庫存
        /// </summary>
        /// <param name="order"></param>
        /// <param name="cond"></param>
        private void CancelInventoryQty(OrderDto order, UpdateStatusCondition cond)
        {
            var orderDeliveries = order.OrderDeliverys;
            var skuHisList = new List<Guid>();
            if (cond == null)
            {
                if (order.OrderDeliverys == null || order.OrderDeliverys.Count == 0)
                {
                    var dbDelivery = baseRepository.GetList<OrderDelivery>(d => d.OrderId == order.Id).ToList();
                    orderDeliveries = AutoMapperExt.MapTo<List<OrderDeliveryDto>>(dbDelivery);
                }

                //  SaveDebug(GetType().FullName, ClassUtility.GetMethodName(), "orderDeliveries qty=", orderDeliveries.Count().ToString());
                foreach (var item in orderDeliveries)
                {
                    //  SaveDebug(GetType().FullName, ClassUtility.GetMethodName(), "DeliveryDetails qty=", item.DeliveryDetails.Count().ToString());
                    foreach (var product in item.DeliveryDetails)
                    {
                        var reserve = new InventoryReservedDto();
                        reserve.Sku = product.SkuId;
                        reserve.WHId = Guid.Empty;
                        reserve.SubOrderId = item.Id;
                        reserve.OrderId = order.Id;
                        reserve.ReservedQty = product.Qty;

                        //  SaveDebug(GetType().FullName, ClassUtility.GetMethodName(), "InventoryBLL.CancelInvReserved", JsonUtil.Serialize(reserve));
                        //当订单状态为已付款、处理中、已送货其中一种时才取消预留
                        if (cond != null && (cond.CurrentStatus == OrderStatus.PaymentConfirmed || cond.CurrentStatus == OrderStatus.Processing || cond.CurrentStatus == OrderStatus.DeliveryArranged))//当订单支付后才有预留，所以取消状态为支付后的订单时才取消预留
                        {
                            var result = inventoryBLL.CancelInvReserved(reserve);
                            if (!result.Succeeded) throw new BLException(result.Message);                            
                        }

                        if (!skuHisList.Contains(product.SkuId))
                        {
                            InventoryHold invHold = new InventoryHold
                            {
                                SkuId = product.SkuId,
                                MemberId = order.MemberId,
                            };

                            var resultHold = inventoryBLL.DeleteInventoryHold(invHold);
                            if (!resultHold.Succeeded)
                            {
                                throw new BLException(resultHold.Message);
                            }
                            else
                            {
                                skuHisList.Add(product.SkuId);
                            }
                        }
                    }
                }
            }
            else//根據送貨單去取消預留
            {

                if (cond.DeliveryTrackingInfo != null && cond.DeliveryTrackingInfo.Count > 0)
                {
                    var deliveryInfo = cond.DeliveryTrackingInfo.FirstOrDefault();

                    var delivery = orderDeliveries.FirstOrDefault(p => p.Id == deliveryInfo.Id);

                    foreach (var product in delivery.DeliveryDetails)
                    {
                        //当订单状态为已付款、处理中、已送货其中一种时才取消预留
                        if (cond != null && (cond.CurrentStatus == OrderStatus.PaymentConfirmed || cond.CurrentStatus == OrderStatus.Processing || cond.CurrentStatus == OrderStatus.DeliveryArranged))//当订单支付后才有预留，所以取消状态为支付后的订单时才取消预留
                        {
                            var reserve = new InventoryReservedDto();
                            reserve.Sku = product.SkuId;
                            reserve.WHId = Guid.Empty;
                            reserve.SubOrderId = delivery.Id;
                            reserve.OrderId = order.Id;
                            reserve.ReservedQty = product.Qty;

                            var result = inventoryBLL.CancelInvReserved(reserve);
                            if (!result.Succeeded)
                            {
                                throw new BLException(result.Message);
                            }
                        }

                        if (!skuHisList.Contains(product.SkuId))
                        {
                            InventoryHold invHold = new InventoryHold
                            {
                                SkuId = product.SkuId,
                                MemberId = order.MemberId,
                            };

                            var resultHold = inventoryBLL.DeleteInventoryHold(invHold);
                            if (!resultHold.Succeeded)
                            {
                                throw new BLException(resultHold.Message);
                            }
                            else
                            {
                                skuHisList.Add(product.SkuId);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 更新產品銷售數量匯總數據
        /// </summary>
        /// <param name="order">訂單信息</param>
        private void UpdateMerchantSalesStatistic(OrderDto order)
        {
            var orderDtlLst = order.OrderDetails;
            if (orderDtlLst?.Count > 0)
            {
                var keyPairMerchQty = new Dictionary<Guid, int>();

                foreach (var orderDtl in orderDtlLst)
                {
                    var merchId = orderDtl.MerchantId;
                    int qty = orderDtl.Qty;

                    if (!keyPairMerchQty.Keys.Contains(merchId))
                    {
                        keyPairMerchQty.Add(merchId, qty);
                    }
                    else
                    {
                        int newQty = keyPairMerchQty[merchId] + qty;
                        keyPairMerchQty[merchId] = newQty;
                    }
                }

                foreach (var pair in keyPairMerchQty)
                {
                    var merchStatistic = baseRepository.GetModel<MerchantSalesStatistic>(x => x.IsActive && !x.IsDeleted && x.MerchantId == pair.Key);
                    if (merchStatistic != null)
                    {
                        merchStatistic.Qty += pair.Value;
                        baseRepository.Update(merchStatistic);
                    }
                    else
                    {
                        merchStatistic = new MerchantSalesStatistic()
                        {
                            MerchantId = pair.Key,
                            Qty = pair.Value
                        };
                        baseRepository.Insert(merchStatistic);
                    }
                }
            }
        }

        private void UpdateProductPurchaseStatistic(OrderDto order)
        {
            var orderDtlLst = order.OrderDetails;
            string key = $"{PreHotType.Hot_PreProductStatistics}";
            if (orderDtlLst?.Count > 0)
            {
                List<ProductStatistics> statistics = new List<ProductStatistics>();
                foreach (var item in orderDtlLst)
                {
                    var product= baseRepository.GetModel<Product>(x=>x.Id == item.ProductId);
                    var statistic = baseRepository.GetModel<ProductStatistics>(x=>x.Code == product.Code);
                    if (statistic != null)
                    {
                        statistic.PurchaseCounter += 1;
                        statistic.UpdateDate = DateTime.Now;

                        //statistics.Add(statistic);
                        baseRepository.Update(statistic);
                    }
                    else
                    {
                        statistic = new ProductStatistics();
                        statistic.Id = Guid.NewGuid();

                        statistic.Code = product.Code;
                        statistic.CreateBy = Guid.Parse(CurrentUser.UserId);
                        statistic.CreateDate = DateTime.Now;
                        statistic.PurchaseCounter = 1;
                        statistic.Score = 0;
                        statistic.ScoreNum = 0;
                        statistic.VisitCounter = 1;

                        baseRepository.Insert(statistic);
                    }

                    var statisticsData = new HotPreProductStatistics
                    {
                        Code = statistic.Code,
                        Score = statistic.Score,
                        PurchaseCounter = statistic.PurchaseCounter,
                        SearchCounter = statistic.SearchCounter,
                        VisitCounter = statistic.VisitCounter
                    };
                    //刷新缓存
                    RedisHelper.HSet(key, statistic.Code, statisticsData);
                }
            }
        }

        private void UpdateStatus(OrderDto order, OrderStatus status, UpdateStatusCondition cond)
        {
            //var orderDeliveries = baseRepository.GetList<OrderDelivery>(p => p.OrderId == order.Id).ToList();
            var deliveries = order.OrderDeliverys;
            var orderDeliveries = AutoMapperExt.MapTo<List<OrderDelivery>>(deliveries);
            if (orderDeliveries.Any())
            {
                if (cond == null)//如果指定的送货单信息为空，表示订单下所有的送货单都更新相同的状态
                {
                    //只有过期取消订单时才会执行到此逻辑
                    if (status == OrderStatus.SCancelled || status == OrderStatus.ECancelled)//如果傳入的cond為空且是更新為取消訂單的，則訂單和訂單下所有送貨單的狀態都改為取消
                    {
                        order.Status = status;
                        foreach (var item in orderDeliveries)
                        {
                            item.Status = status;
                            item.UpdateDate = DateTime.Now;
                            InsertSubOrderStatusHistory(order.Id, item.Id, status);

                        }
                    }
                }
                else if (cond.DeliveryTrackingInfo != null && cond.DeliveryTrackingInfo.Any())//有指定的更新送货单信息
                {

                    var deliveryInfo = cond.DeliveryTrackingInfo.FirstOrDefault();
                    var deliveryData = orderDeliveries.FirstOrDefault(p => p.Id == deliveryInfo.Id);
                    if (deliveryData != null)
                    {
                        if (CurrentUser.IsMerchant || deliveryData.MerchantId == CurrentUser.MerchantId)
                        {
                            if (status == OrderStatus.DeliveryArranged)
                            {
                                //var deliveryInfo = cond.DeliveryTrackingInfo.FirstOrDefault(p => p.Id == item.Id);
                                deliveryData.LocationId = deliveryInfo.LocationId;
                                deliveryData.TrackingNo = GetTrackingNo(deliveryData, deliveryInfo);

                                var trackingNoList = deliveryData.TrackingNo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var trackingNO in trackingNoList)//如果是EMS的快遞單，有機會出現一章送貨單對應多張快遞單號
                                {
                                    UpdateDeliveryDetailTrackingNo(deliveryData, trackingNO);
                                }
                            }
                            deliveryData.UpdateDate = DateTime.Now;
                            deliveryData.UpdateBy = Guid.Parse(CurrentUser.UserId);
                            deliveryData.Status = status;

                            InsertSubOrderStatusHistory(order.Id, deliveryData.Id, status);
                            orderDeliveryRepository.UpdateOrderDeliveryStatus(deliveryData);
                        }
                        OrderStatus orderStatus = orderDeliveries.Min(m => m.Status);
                        order.Status = orderStatus;
                        order.UpdateDate = DateTime.Now;
                    }
                }
                if (status == OrderStatus.PaymentConfirmed) order.IsPaid = true;
            }

            var dbOrder = AutoMapperExt.MapTo<Order>(order);
            baseRepository.Update(dbOrder);

            if (orderDeliveries.Any())
            {
                if (cond == null)
                {
                    foreach (var item in orderDeliveries)
                    {
                        UpdateOrderMassProcessStatus(item, status);
                    }
                }
                else
                {
                    var deliveryInfo = cond.DeliveryTrackingInfo.FirstOrDefault();
                    var deliveryData = orderDeliveries.FirstOrDefault(p => p.Id == deliveryInfo.Id);
                    UpdateOrderMassProcessStatus(deliveryData, status);
                }
            }
            if (status == order.Status)
            {
                InsertOrderStatusHistory(order.Id, status);
            }
        }

        /// <summary>
        /// 更新MassProcess状态
        /// </summary>
        /// <param name="item"></param>
        /// <param name="status"></param>
        private void UpdateOrderMassProcessStatus(OrderDelivery item, OrderStatus status)
        {
            //// 应用于之前邮局的内部商家，现在没有用暂时注释
            //var ecshipStatusRecord = UnitOfWork.DataContext.OrderMassProcessStatus.FirstOrDefault(p => p.DeliveryId == item.Id);
            if (status == OrderStatus.DeliveryArranged && item.Status == status)
            {
                orderDeliveryRepository.UpdateOrderDeliveryArrangedStatus(item);              
            }
            else
            {           
                orderDeliveryRepository.UpdateOrderDeliveryStatus(item);
            }
        }

        private string GetTrackingNo(OrderDelivery dbDelivery, DeliveryTrackingInfo deliveryInfo)
        {
            var express = deliveryBLL.GetExpressItem(dbDelivery.ExpressCompanyId);
            if (express?.Code == "LC")
            {
                var trackingNo = "";
                if (dbDelivery.TrackingNo.IsEmpty())
                {
                    var result = AutoGenerateNumber("LC");
                }
                else
                {
                    trackingNo = dbDelivery.TrackingNo;
                }


                return trackingNo;
            }
            else
            {
                return deliveryInfo.TrackingNo;
            }
        }

        private void AddInventoryReserved(OrderDto order, out List<Guid> skuIdList)
        {
            skuIdList = new List<Guid>();
            //todo check inventory
            bool isInventoryEnable = true;
            if (isInventoryEnable)
            {
                var orderDeliveryList = order.OrderDeliverys;
                foreach (var orderDelivery in orderDeliveryList)
                {
                    var deliveryItems = orderDelivery.DeliveryDetails.GroupBy(g => g.SkuId).Select(d => new
                    {
                        SkuId = d.Key,
                        Qty = d.Sum(a => a.Qty)
                    }).ToList();

                    foreach (var item in deliveryItems)
                    {
                        if (!skuIdList.Contains(item.SkuId))
                        {
                            skuIdList.Add(item.SkuId);
                        }

                        var orderDetail = order.OrderDetails.FirstOrDefault(p => p.SkuId == item.SkuId);
                        if (orderDetail != null)
                        {                
                            var reserved = new InventoryReservedDto();
                            reserved.Sku = item.SkuId;
                            reserved.ReservedQty = item.Qty;
                            reserved.OrderId = order.Id;
                            reserved.SubOrderId = orderDelivery.Id;

                            var reservedResulst = inventoryBLL.AddInvReserved(reserved, order.MemberId);
                            if (!reservedResulst.Succeeded)
                            {
                                var p = baseRepository.GetModel<Product>(x=>x.Id == orderDetail.ProductId);
                                throw new BLException(Resources.Message.Sellout + ":[" + p?.Code ?? "" + "]");
                            }

                            #region 刪除庫存保留記錄

                            var holdCond = new InventoryHold()
                            {
                                SkuId = item.SkuId,
                                MemberId = order.MemberId
                            };
                            var exsitInvtHold = inventoryBLL.IsExsitInventoryHold(holdCond);//檢查是否存在庫存保留記錄
                            if (exsitInvtHold.Succeeded)
                            {
                                //是則在添加預留後，刪除庫存保留記錄
                                var resultDelHold = inventoryBLL.DeleteInventoryHold(holdCond);
                                if (!resultDelHold.Succeeded) throw new BLException(Resources.Message.DeleteInvtHoldFailed + " skuId:" + item.SkuId);                                
                            }

                            #endregion
                        }
                    }
                }
            }
        }

        private void UpdateDeliveryDetailTrackingNo(OrderDelivery delivery, string trackingNo)
        {

            var deliveryDetails = baseRepository.GetList<OrderDeliveryDetail>(p => p.DeliveryId == delivery.Id).OrderBy(o => o.Id).ToList();
            foreach (var item in deliveryDetails)
            {
                item.TrackingNo = trackingNo;
                item.LocationId = delivery.LocationId;
                item.UpdateBy = Guid.Parse(CurrentUser.UserId);
                item.UpdateDate = DateTime.Now;
                
            }
            baseRepository.Update(deliveryDetails);
        }

        /// <summary>
        /// 更新產品銷售數量匯總數據
        /// </summary>
        /// <param name="order">訂單信息</param>
        private void UpdateProductSaleSummary(OrderDto order)
        {
            if (order == null) { return; }
            var orderDtlLst = order.OrderDetails.Where(x=>x.SkuId !=Guid.Empty).ToList();
            if (orderDtlLst.Any())
            {
                DateTime today = DateTime.Now.Date;
                int thisYear = today.Year;
                int thisMonth = today.Month;
                int thisDay = today.Day;

                foreach (var orderDtl in orderDtlLst)
                {                    
                    var merchId = orderDtl.MerchantId;
                    var hotSalesSummary = baseRepository.GetModel<ProductSalesSummry>(x =>x.MerchantId == merchId && x.IsActive && !x.IsDeleted
                                                         && x.Year == thisYear && x.Month == thisMonth && x.Day == thisDay&& x.Sku == orderDtl.SkuId);
                    if (hotSalesSummary != null)
                    {
                        hotSalesSummary.Qty += orderDtl.Qty;
                        baseRepository.Update(hotSalesSummary);
                    }
                    else
                    {
                        var hotSalesSum = new ProductSalesSummry()
                        {
                            MerchantId = merchId,
                            Sku = orderDtl.SkuId,
                            Year = thisYear,
                            Month = thisMonth,
                            Day = thisDay,
                            Qty = orderDtl.Qty,
                        };
                        baseRepository.Insert(hotSalesSum);
                    }
                }
            }

        }

        /// <summary>
        /// 更新订单状态为处理中
        /// </summary>
        /// <param name="order"></param>
        /// <param name="cond"></param>
        public void UpdateOrderStatusToProcess(OrderDto order, UpdateStatusCondition cond)
        {
            if (CheckDeliveryStautsIsSame(cond)) UpdateStatus(order, OrderStatus.Processing, cond);
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="orderView"></param>
        /// <returns></returns>
        private SystemResult CreateOrder(NewOrder orderView)
        {
            SystemResult result = new SystemResult();
            List<string> transinCodes = new List<string>();
            var postFix = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            if (orderView.Items.Count > postFix.Count) throw new BLException(string.Format(Resources.Message.SubOrderNumNotGreaterThan, postFix.Count));
            
            if (!CheckOrderQtyIsCorrect(orderView)) throw new BLException(Resources.Message.ShoppingCartItemChanged);
           
            if (!CheckDiscountRule(orderView)) throw new BLException(Resources.Message.FailToUseDiscount);
            
            var discountMessage = CheckOrderDiscount(orderView);
            if (!discountMessage.IsEmpty()) throw new BLException(discountMessage);

            var items = baseRepository.GetList<ShoppingCartItem>(d => d.MemberId == Guid.Parse(CurrentUser.UserId) && d.IsActive && !d.IsDeleted).ToList();
            if (!items?.Any() ?? false) throw new BLException(Resources.Message.NotValidProduct);
            
            decimal totalWeightKG = 0;
            string orderNo = AutoGenerateNumber("");
            var baseCurrency = currencyBLL.GetDefaultCurrency();

            var order = new OrderDto();
            order.Status = OrderStatus.ReceivedOrder;
            order.Currency = baseCurrency;
            order.Id = Guid.NewGuid();
            order.OrderNO = orderNo;
            orderView.OrderNO = orderNo;//返回調用的方法用
            order.MemberId = Guid.Parse(CurrentUser.UserId);
            order.ExchangeRate = baseCurrency.ExchangeRate;//查匯率
            order.PaymentMethodId = orderView.PaymentMethodId;
            order.CurrencyCode = order.Currency?.Code;

            UnitOfWork.IsUnitSubmit = true;

            #region 处理订单明细，商品时段价格流水账，购物车数据
            foreach (var item in items)
            {               
                Product p = baseRepository.GetModelById<Product>(item.ProductId);
                var productSpecifications = baseRepository.GetModel< ProductSpecification>(x=>x.Id == item.ProductId);
                if (p.Status != ProductStatus.OnSale || p.IsDeleted) throw new BLException(Resources.Message.ProductExpired + p.Code);

                #region 处理订单明细
                var addPrices = productRepository.GetProductAddPriceBySku(item.ProductId, item.SkuId);
                //create order detail record
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.Id = Guid.NewGuid();
                orderDetail.OrderId = order.Id;
                orderDetail.SkuId = item.SkuId;
                orderDetail.ProductId = item.ProductId;//for order history
                orderDetail.SalePrice = p.SalePrice + p.MarkUpPrice;//价钱添加MarkUpPrice
                orderDetail.OriginalPrice = p.OriginalPrice + p.MarkUpPrice;
                orderDetail.Qty = item.Qty;
                orderDetail.MerchantId = p.MerchantId;
                orderDetail.AddPrice1 = addPrices.Count > 0 ? addPrices[0] : 0;
                orderDetail.AddPrice2 = addPrices.Count > 1 ? addPrices[1] : 0;
                orderDetail.AddPrice3 = addPrices.Count > 2 ? addPrices[2] : 0;
                order.OrderDetails.Add(orderDetail);
                #endregion

                totalWeightKG += productSpecifications.WeightUnit == WeightUnit.G ? productSpecifications.GrossWeight / 1000 : productSpecifications.GrossWeight;

                #region 处理时段价格变化流水账
                OrderPriceDetail orderPriceDetail = new OrderPriceDetail();
                ProductPriceHour productPriceHour = GetProductPriecHour(p);
                if (productPriceHour != null)
                {
                    orderPriceDetail.Id = Guid.NewGuid();
                    orderPriceDetail.OrderId = order.Id;
                    orderPriceDetail.SkuId = item.SkuId;
                    orderPriceDetail.ProductId = item.ProductId;

                    orderPriceDetail.SalePrice = p.SalePrice;
                    orderPriceDetail.TimePrice = p.TimePrice;
                    orderPriceDetail.BeginTime = productPriceHour.BeginTime;
                    orderPriceDetail.EndTime = productPriceHour.EndTime;
                    order.OrderPriceDetails.Add(orderPriceDetail);
                }
                #endregion

                item.IsDeleted = true;
                baseRepository.Update(item);    
            }
            baseRepository.Insert(order.OrderPriceDetails);
            baseRepository.Insert(order.OrderDetails);

            #endregion

            //訂單的總重量以KG為單位
            order.TotalWeight = totalWeightKG;
            //计算产品中多件优惠的优惠价格
            var productDiscount = CalculateProductGroupSaleDiscount(order, orderView.Items);

            #region 送货单
            //送貨單
            foreach (var item in orderView.Items)
            {
                int index = orderView.Items.IndexOf(item);
                var address = deliveryAddressBLL.GetAddress(item.AddressId);

                var orderDelivery = new OrderDeliveryDto();
                orderDelivery.Id = item.DeliveryId;//Guid.NewGuid();           
                orderDelivery.OrderId = order.Id;               
                orderDelivery.DeliveryNO = orderNo + postFix[index];
                orderDelivery.CountryCode = item.CountryCode;
                orderDelivery.Remark = item.Remark ?? "";
                orderDelivery.Status = OrderStatus.ReceivedOrder;
                orderDelivery.LocationId = Guid.Empty;
                orderDelivery.TrackingNo = "";
                orderDelivery.DeliveryType = item.DeliveryType;
                orderDelivery.ServiceType = item.ServiceType;

                #region 设置送货方式
                if (item.DeliveryType == DeliveryType.D)
                {
                    orderDelivery.Recipients = (address?.FirstName ?? "") + " " + (address?.LastName ?? "");
                    orderDelivery.Address = address?.Address ?? "";
                    orderDelivery.Address1 = address?.Address1 ?? "";
                    orderDelivery.Address2 = address?.Address2 ?? "";
                    orderDelivery.Address3 = address?.Address3 ?? "";
                    orderDelivery.ContactPhone = address?.Phone ?? "";
                    orderDelivery.Country = address == null ? "" : NameUtil.GetCountryName(CurrentUser.Lang.ToString(), deliveryAddressBLL.GetCountry(address.CountryId));
                    orderDelivery.Province = address == null ? "" : NameUtil.GetProviceName(CurrentUser.Lang.ToString(), deliveryAddressBLL.GetProvince(address.ProvinceId));
                    orderDelivery.City = address?.City ?? "";
                    orderDelivery.PostalCode = address?.PostalCode ?? "";
                    orderDelivery.PLType = 0;
                    orderDelivery.ExpressCompanyId = item.ExpressCompanyId;
                    orderDelivery.Email = address.Email;
                    orderDelivery.Status = OrderStatus.ReceivedOrder;
                }
                else
                {
                    //ExpressPickUp expressPickUp = _expressPickUpRepository.GetExpressPickUpByCode(item.MCNCode);
                    //orderDelivery.Recipients = item.ContactName ?? "";
                    //orderDelivery.ContactPhone = item.ContactPhone ?? "";
                    //orderDelivery.Email = item.ContactEmail ?? "";
                    //orderDelivery.Address = expressPickUp?.Address ?? "";
                    //orderDelivery.Country = expressPickUp?.Country ?? "";
                    //orderDelivery.Province = expressPickUp?.Province ?? "";
                    //orderDelivery.City = expressPickUp?.City ?? "";

                    //orderDelivery.MCNCode = item.MCNCode;
                    //orderDelivery.ExpressCompanyId = item.ExpressCompanyId;
                }

                #endregion

                //送貨單明細，一張訂單，一張送貨單               
                foreach (var od in item.Detail)
                {
                    if (od.Qty > 0)//判断送货单的每个产品的数量是否大于0
                    {
                        var orderDetail = order.OrderDetails.FirstOrDefault(d => d.SkuId == od.Sku);                   
                        orderDelivery.MerchantId = orderDetail.MerchantId;
                        var odd = new OrderDeliveryDetailDto();
                        odd.Id = Guid.NewGuid();
                       
                        odd.DeliveryId = orderDelivery.Id;
                        odd.ProductId = orderDetail.ProductId;
                        odd.SkuId = od.Sku;
                        odd.SalePrice = orderDetail.SalePrice;
                        odd.OriginalPrice = orderDetail.OriginalPrice;
                        odd.Qty = od.Qty;
                        odd.IsFree = od.IsFree;
                        odd.RuleId = od.RuleId;
                        odd.HasFree = item.Detail.Where(p => p.Sku == od.Sku && p.RuleId == Guid.Empty && p.IsFree == true).Count() > 0;
                        odd.SetPrice = od.IsFree ? 0 : GetSetPrice(odd, od.RuleId);
                        odd.AddPrice1 = orderDetail.AddPrice1;
                        odd.AddPrice2 = orderDetail.AddPrice2;
                        odd.AddPrice3 = orderDetail.AddPrice3;
                        odd.OrderId = orderDelivery.OrderId;
                        orderDelivery.DeliveryDetails.Add(odd);

                        if (od.RuleId != Guid.Empty)//将PromotionRule 补充入DiscountView
                        {
                            var discount = productDiscount.FirstOrDefault(p => p.DeliveryId == orderDelivery.Id && p.ProductId == od.Sku);
                            if (discount != null)
                            {
                                var promotionRule = new DiscountView();
                                promotionRule.Id = od.RuleId;
                                promotionRule.CouponType = CouponUsage.Price;
                                promotionRule.DiscountType = DiscountType.PromotionRule;
                                promotionRule.IsPercent = false;
                                promotionRule.DiscountValue = discount.DiscountPrice;
                                promotionRule.DiscountPrice = discount.DiscountPrice;
                                promotionRule.ProductId = odd.ProductId;

                                if (item.Discounts != null)
                                {
                                    item.Discounts.Add(promotionRule);
                                }
                                else
                                {
                                    item.Discounts = new List<DiscountView>();
                                    item.Discounts.Add(promotionRule);
                                }
                            }

                        }
                    }
                }

                orderDelivery.Freight = item.OriginalFreight;//没扣减免运费产品重量的运费
                orderDelivery.FreightDiscount = item.FreightDiscount;//用運費折扣
                orderDelivery.TotalPrice = orderDelivery.DeliveryDetails.Where(p => !p.IsFree).Sum(d => d.Qty * (d.SalePrice + d.AddPrice1 + d.AddPrice2 + d.AddPrice3));
                orderDelivery.Amount = orderDelivery.TotalPrice + orderDelivery.Freight;
                orderDelivery.ItemQty = orderDelivery.DeliveryDetails.Where(p => !p.IsFree).Sum(d => d.Qty);

                //將原總價和原運費的值，作為折扣價和折扣運費的欄位默認值，但沒有優惠時，兩者的值是一致的
                orderDelivery.DiscountPrice = orderDelivery.TotalPrice;
                orderDelivery.DiscountFreight = item.Freight;

                if (orderDelivery.Freight != orderDelivery.DiscountFreight)//表示有免运费产品
                {
                    if (item.Discounts == null)
                    {
                        item.Discounts = new List<DiscountView>();
                    }
                    item.Discounts.Add(new DiscountView
                    {
                        CouponType = CouponUsage.DeliveryCharge,
                        DiscountPrice = orderDelivery.Freight - orderDelivery.DiscountFreight,
                        DiscountType = DiscountType.FreeChargeProduct,
                        DiscountValue = orderDelivery.Freight - orderDelivery.DiscountFreight,
                        Id = Guid.Empty,
                        IsPercent = false,
                        ProductId = Guid.Empty
                    });
                    orderDelivery.FreeShippingFreight = item.Freight;//免運費后的運費
                }
                CalculateSubOrderDiscount(orderDelivery, item);

                foreach (var deliveryProduct in orderDelivery.DeliveryDetails)//如果產品沒有應用優惠，實際支付價格等於SalePrice
                {
                    if (deliveryProduct.PayPrice == 0)
                    {
                        deliveryProduct.PayPrice = deliveryProduct.SalePrice;
                    }
                }
             
                orderDelivery.DiscountAmount = orderDelivery.DiscountPrice + orderDelivery.DiscountFreight;
                orderDelivery.ActualAmount = orderDelivery.DiscountAmount;           
                orderDelivery.CoolDownDay = GetCoolDownDate(orderDelivery.MerchantId, orderDelivery.CountryCode);
                order.OrderDeliverys.Add(orderDelivery);

                InsertSubOrderStatusHistory(order.Id, orderDelivery.Id, OrderStatus.ReceivedOrder);
                InsertSubOrderDiscountRecord(item.Discounts, order.Id, orderDelivery.Id);//插入sub order使用的優惠券

                var odDelivery = AutoMapperExt.MapTo<List<OrderDelivery>>(order.OrderDeliverys);               
                baseRepository.Insert(odDelivery);    // 插入送货单

                //插入ECShipStatus记录
                //InsertECShipStatisRecord(orderDelivery);
            }

            foreach (var item in order.OrderDeliverys)
            {
                var dbDetail =AutoMapperExt.MapTo<List<OrderDeliveryDetail>>( item.DeliveryDetails);
                baseRepository.Insert(dbDetail);        //插入送货单明细
            }

            #endregion

            order.TotalPrice = order.OrderDeliverys.Sum(d => d.TotalPrice);
            order.TotalAmount = order.OrderDeliverys.Sum(d => d.Amount);
            order.Freight = order.OrderDeliverys.Sum(d => d.Freight);
            order.FreightDiscount = order.OrderDeliverys.Sum(d => d.FreightDiscount);
            order.ItemQty = order.OrderDetails.Sum(d => d.Qty);
            order.FreeShippingFreight = order.OrderDeliverys.Sum(d => d.FreeShippingFreight);
            order.DiscountPrice = order.OrderDeliverys.Sum(d => d.DiscountPrice);
            order.DiscountFreight = order.OrderDeliverys.Sum(d => d.DiscountFreight);

            CalculateOrderDiscount(order, orderView);
            order.DiscountAmount = order.DiscountPrice + order.DiscountFreight;

            if (order.DiscountAmount >= orderView.MallFun)
            {
                order.DiscountAmount = order.DiscountAmount - orderView.MallFun;
                UpdateMallFun(orderView.MallFun);

                if (orderView.MallFun > 0)
                {
                    orderView.Discounts.Add(new DiscountView
                    {
                        Id = Guid.Empty,
                        CouponType = CouponUsage.Price,
                        DiscountPrice = orderView.MallFun,
                        DiscountType = DiscountType.MallFun,
                        DiscountValue = orderView.MallFun,
                        IsPercent = false,
                        ProductId = Guid.Empty
                    });
                }
                order.MallFun = orderView.MallFun;
            }
            else
            {
                order.DiscountAmount = order.DiscountAmount - Math.Floor(order.DiscountAmount);//order.DiscountAmount - orderView.MallFun;
                UpdateMallFun(Math.Round(order.DiscountAmount, 2));
                orderView.Discounts.Add(new DiscountView
                {
                    Id = Guid.Empty,
                    CouponType = CouponUsage.Price,
                    DiscountPrice = Math.Floor(order.DiscountAmount),
                    DiscountType = DiscountType.MallFun,
                    DiscountValue = Math.Floor(order.DiscountAmount),
                    IsPercent = false,
                    ProductId = Guid.Empty
                });

                order.MallFun = Math.Round(order.DiscountAmount, 2);
            }

            InsertOrderStatusHistory(order.Id, OrderStatus.ReceivedOrder);
            InsertOrderDiscountRecord(orderView.Discounts, order.Id);

            var dbOrder = AutoMapperExt.MapTo<Order>(order);
            baseRepository.Insert(dbOrder);
            baseRepository.Insert(order.OrderDetails);
            result.Succeeded = true;

            UnitOfWork.Submit();

            #region 加密送货地址数据
            if (result.Succeeded)
            {
                using var tran = baseRepository.CreateTransation();
                var encriptSql = @"OPEN SYMMETRIC KEY AES256key_Do1Mall DECRYPTION BY CERTIFICATE [CERTDO1MAll]; 
                                         update OrderDeliveries set 
                                         Recipients=CONVERT(nvarchar(200),  EncryptByKey(Key_GUID('AES256key_Do1Mall'),Recipients)),
                                         ContactPhone=CONVERT(nvarchar(200),  EncryptByKey(Key_GUID('AES256key_Do1Mall'),ContactPhone)), 
                                         [Address]=CONVERT(nvarchar(200),  EncryptByKey(Key_GUID('AES256key_Do1Mall'),[Address])),
                                         [Address1]=CONVERT(nvarchar(200),  EncryptByKey(Key_GUID('AES256key_Do1Mall'),[Address1])),
                                         [Address2]=CONVERT(nvarchar(200),  EncryptByKey(Key_GUID('AES256key_Do1Mall'),[Address2])),
                                         [Address3]=CONVERT(nvarchar(200),  EncryptByKey(Key_GUID('AES256key_Do1Mall'),[Address3])) 
					                     from OrderDeliveries  where orderId=@orderId ;
                                         CLOSE SYMMETRIC KEY AES256key_Do1Mall;";

                var param = new List<SqlParameter>();
                param.Add(new SqlParameter { ParameterName = "@orderId", Value = order.Id });
                baseRepository.ExecuteSqlCommand(encriptSql, param.ToArray());
                tran.Commit();
                result.Succeeded = true;
            }
            #endregion

            return result;
        }

        private OrderSummaryView GenOrderSummaryView(OrderView order)
        {
            var pm = paymentBLL.GetPaymentMenthod(order.PaymentMethodId);
            OrderSummaryView view = new OrderSummaryView();
            view.Id = order.Id;
            view.OrderNo = order.OrderNO;
            view.IsPay = order.IsPaid;       
            view.Currency = currencyBLL.GetSimpleCurrency(order.CurrencyCode);
            view.Status = order.Status;
            view.StatusName = codeMasterRepository.GetCodeMaster(CodeMasterModule.System.ToString(), CodeMasterFunction.OrderStatus.ToString(), order.Status.ToString())?.Description ?? "";
            view.TotalWeight = order.TotalWeight;

            view.TotalAmount = order.TotalAmount;
            view.TotalQty = order.ItemQty;
            view.DiscountTotalAmount = order.DiscountAmount;

            view.CreateDate = order.CreateDate;
            view.CreateDateString = DateUtil.DateTimeToString(order.CreateDate, "yyyy-MM-dd HH:mm:ss");
            view.PaymentMethod = pm?.Name ?? string.Empty;
            view.PMCode = pm?.Code;
            view.PMRate = pm?.ServRate;
            view.UpdateDate = order.UpdateDate;
            view.UpdateDateString = DateUtil.DateTimeToString(order.UpdateDate, "yyyy-MM-dd HH:mm:ss");
            view.MemberName = order.MemberName;
            //view.MemberName = MemberBLL.GetMember(order.MemberId)?.FullName ?? "";
            return view;
        }

        private OrderInfoView BuildModel(Order dbOrder)
        {
            if (dbOrder == null) return null;
    
            //var countrys = DeliveryAddressBLL.GetCountries();
            //var provinces = DeliveryAddressBLL.GetProvinces(0);
            var member = baseRepository.GetModelById<Member>(dbOrder.MemberId);

            OrderInfoView order = new OrderInfoView();
            order.IsMerchant = CurrentUser.IsMerchant;

            order.MemberId = dbOrder.MemberId;
            order.Member = new SimpleMember()
            {
                Id = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Mobile = member.Mobile,
                Phone = member.Mobile,
                Email = member.Email
            };
            order.OrderId = dbOrder.Id.ToString();
            order.OrderNO = dbOrder.OrderNO;
            order.InvoiceNO = dbOrder.InvoiceNO;
            order.PaymentMethodId = dbOrder.PaymentMethodId;
            var pm = paymentBLL.GetPaymentMenthod(order.PaymentMethodId);
            order.PaymentMethod = pm.Name ?? string.Empty;
            order.PMServRate = pm?.ServRate;
            order.PMCode = pm.Code ?? string.Empty;
            order.ItemsAmount = dbOrder.TotalPrice;
            order.TotalAmount = dbOrder.TotalAmount;
            order.DiscountTotalPrice = dbOrder.DiscountPrice;
            order.DiscountTotalAmount = dbOrder.DiscountAmount;
            order.DiscountDeliveryCharge = dbOrder.DiscountFreight;

            order.TotalQty = dbOrder.ItemQty;
            order.CreateAt = dbOrder.CreateDate;
            order.UpdateAt = dbOrder.UpdateDate.Value;
            order.Currency = currencyBLL.GetSimpleCurrency(dbOrder.CurrencyCode);
            order.ExchangerRate = dbOrder.ExchangeRate;
            order.MallFun = dbOrder.MallFun;
            if (!CurrentUser.IsMerchant)
            {
                order.StatusCode = ((int)dbOrder.Status).ToString();
            }
            else
            {
                order.StatusCode = baseRepository.GetModel<OrderDelivery>(x=>x.OrderId== dbOrder.Id && x.MerchantId == CurrentUser.MerchantId)?.Status.ToInt().ToString();
            }

            order.StatusName = codeMasterRepository.GetCodeMaster(CodeMasterModule.System.ToString(), CodeMasterFunction.OrderStatus.ToString(), dbOrder.Status.ToString())?.Description ?? "";
            order.IsPay = dbOrder.IsPaid;

            order.DeliveryCharge = dbOrder.Freight;

            order.DeliveryDiscount = dbOrder.FreightDiscount;
            order.OrderItems = GetOrderItems(dbOrder);
            order.Deliveries = GetOrderDeliverys(dbOrder);
            order.Discounts = GetOrderDiscount(dbOrder.Id);
            return order;
        }

        private List<OrderItem> GetOrderItems(Order order)
        {
            var orderItems = new List<OrderItem>();
            var orderDetails = baseRepository.GetList<OrderDetail>(x => x.OrderId == order.Id && x.IsActive && !x.IsDeleted).ToList();

            if (orderDetails?.Any() ?? false)
            {
                if (CurrentUser.IsMerchant) orderDetails = orderDetails.Where(p => p.MerchantId == CurrentUser.MerchantId).ToList();
                
                orderItems = orderDetails.Select(item => new OrderItem
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    SkuId = item.SkuId,
                    Product = productBLL.GetProductSummary(item.ProductId, item.SkuId),
                    Qty = item.Qty,
                    UnitPrice = item.SalePrice,
                    SkuInfo = productBLL.GetProductSku(item.SkuId),

                }).ToList();
            }
            return orderItems;
        }

        private List<OrderDeliveryInfo> GetOrderDeliverys(Order order)
        {

            var list = new List<OrderDeliveryInfo>();
            List<OrderDelivery> deliveries = new List<OrderDelivery>();

            //暂时注释
            //if (CurrentUser.IsMerchant)
            //{                
            //    deliveries = orderDeliveryRepository.Search(order.Id, CurrentUser.MerchantId);
            //}
            //else
            //{         
            //    deliveries = orderDeliveryRepository.Search(order.Id, Guid.Empty);
            //}

            //if (deliveries != null)
            //{
            //    foreach (var item in deliveries)
            //    {

            //        item.CoolDownDay = item.CoolDownDay == 0 ? GetCalmeDate() : item.CoolDownDay;
            //        list.Add(GenOrderDeliveryView(item));
            //    }
            //}


            return list;
        }

        private List<DiscountView> GetOrderDiscount(Guid id)
        {            
            var discounts = baseRepository.GetList<OrderDiscount>(p => p.IsActive && !p.IsDeleted && p.OrderId == id && p.SubOrderId == Guid.Empty).ToList();
            var result = discounts.Select(item => new DiscountView
            {
                CouponType = item.DiscountUsage,
                DiscountPrice = item.DiscountPrice,
                DiscountType = item.DiscountType,
                DiscountValue = item.DiscountValue,
                Id = item.DiscountId,
                IsPercent = item.IsPercent,
                ProductId = item.ProductId,
                Code = item.Code,

            }).ToList();
            return result;
        }

        /// <summary>
        /// 检查订单购买的产品数量是否等于购物车的数量
        /// </summary>
        /// <param name="orderView"></param>
        /// <returns></returns>
        private bool CheckOrderQtyIsCorrect(NewOrder orderView)
        {
            bool result = true;

            var orderProductCount = 0;
            var shoppingCartCount = 0;

            var shoppingCartItems = baseRepository.GetList<ShoppingCartItem>(d => d.MemberId == Guid.Parse(CurrentUser.UserId) && d.IsActive && !d.IsDeleted).ToList();

            if (orderView.Items != null)
            {
                foreach (var item in orderView.Items)
                {
                    var products = item.Detail.Where(p => p.IsFree == false).ToList();
                    foreach (var product in products)
                    {
                        orderProductCount += product.Qty;
                    }
                }
            }

            foreach (var item in shoppingCartItems)
            {

                shoppingCartCount += item.Qty;
            }

            if (orderProductCount != shoppingCartCount)
            {
                result = false;
            }

            return result;

        }

        /// <summary>
        /// 检查优惠卷规则
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private bool CheckDiscountRule(NewOrder order)
        {
            bool result = true;

            bool hasPromotionRule = false;

            int priceCouponCount = 0;
            int priceCouponMerchCount = 0;

            priceCouponCount += order.Discounts.Where(p => p.CouponType == CouponUsage.Price).Count();

            foreach (var item in order.Items)
            {
                if (item.Detail.Where(p => p.RuleId != Guid.Empty).Count() > 0)
                {
                    hasPromotionRule = true;
                }
                if (item.Discounts != null)
                {
                    priceCouponMerchCount += item.Discounts.Where(p => p.CouponType == CouponUsage.Price).Count();
                }
            }

            ///如果同時存在現金券、推廣規則。表示優惠規則有問題
            if (hasPromotionRule == true && priceCouponCount > 0)result = false;
            
            if (priceCouponCount > 1 && priceCouponMerchCount > 0)result = false;
            
            var memberAccount = baseRepository.GetModel<MemberAccount>(x => x.MemberId == Guid.Parse(CurrentUser.UserId));

            if ((memberAccount?.Fun  ?? 0)< order.MallFun)  result = false;
            
            return result;
        }

        private string CheckOrderDiscount(NewOrder orderView)
        {
            string result = string.Empty;
            if (orderView.Discounts != null)
            {
                result = CheckDiscountIsActive(orderView.Discounts);

                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
            }

            if (orderView.Items != null)
            {
                foreach (var item in orderView.Items)
                {
                    if (item.Discounts != null)
                    {
                        result = CheckDiscountIsActive(orderView.Discounts);

                        if (!string.IsNullOrEmpty(result))
                        {
                            return result;
                        }
                    }
                }

                foreach (var item in orderView.Items)
                {
                    if (item.Detail != null)
                    {
                        List<DiscountView> discounts = new List<DiscountView>();
                        foreach (var discount in item.Detail)
                        {
                            discounts.Add(new DiscountView
                            {
                                Id = discount.RuleId,
                                DiscountType = DiscountType.PromotionRule
                            });
                        }

                        if (discounts.Count > 0)
                        {
                            result = CheckDiscountIsActive(discounts);

                            if (!string.IsNullOrEmpty(result))
                            {
                                return result;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private string CheckDiscountIsActive(List<DiscountView> discounts)
        {
            string result = string.Empty;
            foreach (var orderDis in discounts)
            {
                bool isActive = true;
                switch (orderDis.DiscountType)
                {
                    case DiscountType.Coupon:
                        var coupon = baseRepository.GetModelById<Coupon>(orderDis.Id);
                        if (coupon != null)
                        {
                            if ((!coupon.IsActive || coupon.IsDeleted) || (coupon.EffectDateFrom > DateTime.Now || coupon.EffectDateTo < DateTime.Now))
                            {
                                var rule = baseRepository.GetModelById<CouponRule>(coupon.RuleId);
                                var ruleTitle = translationRepository.GetDescForLang(rule.TitleTransId,CurrentUser.Lang);
                                isActive = false;
                                result = string.Format(Resources.Message.DiscountExpired, ruleTitle);
                            }
                        }

                        break;
                    case DiscountType.PromotionCode:
                        var promotionCode = baseRepository.GetModelById<PromotionCodeCoupon>(orderDis.Id);
                        if (promotionCode != null)
                        {
                            if ((!promotionCode.IsActive || promotionCode.IsDeleted) || (promotionCode.EffectDateFrom > DateTime.Now || promotionCode.EffectDateTo < DateTime.Now))
                            {
                                var title = translationRepository.GetDescForLang(promotionCode.TitleTransId,CurrentUser.Lang);
                                isActive = false;
                                result = string.Format(Resources.Message.DiscountExpired, title);
                            }
                        }
                        break;
                    case DiscountType.PromotionRule:
                        var promotionRule = baseRepository.GetModelById<PromotionRule>(orderDis.Id);
                        if (promotionRule != null)
                        {
                            if ((!promotionRule.IsActive || promotionRule.IsDeleted) || (promotionRule.EffectDateFrom > DateTime.Now || promotionRule.EffectDateTo < DateTime.Now))
                            {
                                var title = translationRepository.GetDescForLang(promotionRule.TitleTransId, CurrentUser.Lang);
                                isActive = false;
                                result = string.Format(Resources.Message.DiscountExpired, title);
                            }
                        }
                        break;
                }

                if (!isActive)
                {
                    return result;
                }
            }
            return result;
        }

        //获取时段价格
        private ProductPriceHour GetProductPriecHour(Product p)
        {
            ProductPriceHour ppHour = null;
            if (p.SalePrice != p.TimePrice)
            {
                ppHour = baseRepository.GetModel<ProductPriceHour>(d => !d.IsDeleted
                                          && d.MerchantId == p.MerchantId
                                          && d.ProductCode == p.Code
                                          && d.IsActive
                                          && d.IsTimeStatus
                                          && DateTime.Now >= d.BeginTime
                                          && DateTime.Now <= d.EndTime
                                          );
            }

            return ppHour;
        }

        /// <summary>
        /// 计算PromotionRule下，每个产品的优惠价格
        /// </summary>
        /// <param name="order"></param>
        /// <param name="checkoutItems"></param>
        /// <returns></returns>
        private List<PRDiscountPrice> CalculateProductGroupSaleDiscount(OrderDto order, List<CheckoutItem> checkoutItems)
        {
         
                List<OrderDeliveryItemView> buyItems = new List<OrderDeliveryItemView>();
                foreach (var item in checkoutItems)//获取所有是GroupSale的产品
                {
                    foreach (var product in item.Detail)
                    {
                        if (product.RuleType == PromotionRuleType.GroupSale)
                        {
                            buyItems.Add(new OrderDeliveryItemView
                            {
                                DeliveryId = item.DeliveryId,
                                IsFree = product.IsFree,
                                Qty = product.Qty,
                                RuleId = product.RuleId,
                                RuleType = product.RuleType,
                                Sku = product.Sku,
                            });
                        }
                    }

                }


                var groupBuyItems = buyItems.Where(p => p.Qty > 0).GroupBy(g => new { g.Sku, g.RuleId }).Select(d => new
                {
                    //DeliveryId = d.Key.DeliveryId,
                    RuleId = d.Key.RuleId,
                    Sku = d.Key.Sku,
                    Qty = d.Sum(s => s.Qty)
                }).ToList();//将分拆的产品Group起来


                List<PromotionRuleDiscountView> discountView = new List<PromotionRuleDiscountView>();

                foreach (var item in groupBuyItems)//获取所有PromotionRule的信息
                {
                    var orderDetail = order.OrderDetails.SingleOrDefault(d => d.SkuId == item.Sku);

                    discountView.Add(GetPromotionRule(item.RuleId, orderDetail));
                }

                discountView = discountView.GroupBy(g => new { g.Id, g.ProductId }).Select(d => new PromotionRuleDiscountView
                {
                    Id = d.Key.Id,
                    ProductId = d.Key.ProductId,
                    DiscountPrice = d.Select(a => a.DiscountPrice).FirstOrDefault(),
                    SingleDiscountPrice = d.Select(a => a.SingleDiscountPrice).FirstOrDefault()
                }).ToList();

                List<PRDiscountPrice> result = new List<PRDiscountPrice>();


                //foreach (var delivery in checkoutItems)
                //{
                //    delivery.de
                foreach (var discount in discountView)//根据有对少种PromtionRule进行遍历
                {

                    decimal usedSingleDiscount = 0;

                    var ruleProducts = buyItems.Where(p => p.RuleId == discount.Id && p.Sku == discount.ProductId && p.Qty > 0).ToList();

                    var endFlag = 0;//用于辨识一个Rule中是否到达最后一个产品，用于同一个Rule同一种产品进行拆分

                    for (int i = 0; i < ruleProducts.Count; i++)
                    {
                        var ruleProduct = ruleProducts[i];//产品

                        if (endFlag == ruleProducts.Count - 1)//到最后一件时，每件产品优惠价钱计算略不同
                        {
                            if (ruleProducts.Count == 1)//如果是一件产品，那么每件产品的价格是SingleDiscountPrice
                            {
                                result.Add(new PRDiscountPrice
                                {
                                    DeliveryId = ruleProduct.DeliveryId,
                                    ProductId = ruleProduct.Sku,
                                    PromotionRuleId = ruleProduct.RuleId,
                                    DiscountPrice = discount.DiscountPrice,
                                    SingleDiscountPrice = discount.DiscountPrice
                                });
                            }
                            else
                            {

                                decimal lastDiscountPrice = 0;

                                //每个送货单的产品有机会有多个，所以不能discount.DiscountPrice - usedSingleDiscount//优惠的价钱-之前累计的单个产品优惠价钱)，要遍历到最后一个才用discount.DiscountPrice - usedSingleDiscount//优惠的价钱-之前累计的单个产品优惠价钱)
                                if (ruleProduct.Qty > 1)
                                {
                                    lastDiscountPrice = ((ruleProduct.Qty - 1) * discount.SingleDiscountPrice) + discount.DiscountPrice - (usedSingleDiscount + ((ruleProduct.Qty - 1) * discount.SingleDiscountPrice));
                                }
                                else
                                {
                                    lastDiscountPrice = discount.DiscountPrice - usedSingleDiscount;
                                }

                                result.Add(new PRDiscountPrice
                                {
                                    DeliveryId = ruleProduct.DeliveryId,
                                    ProductId = ruleProduct.Sku,
                                    PromotionRuleId = ruleProduct.RuleId,
                                    DiscountPrice = lastDiscountPrice,
                                    SingleDiscountPrice = discount.DiscountPrice - usedSingleDiscount//优惠的价钱-之前累计的单个产品优惠价钱)
                                });
                            }
                        }
                        else
                        {
                            result.Add(new PRDiscountPrice
                            {
                                DeliveryId = ruleProduct.DeliveryId,
                                ProductId = ruleProduct.Sku,
                                PromotionRuleId = ruleProduct.RuleId,
                                DiscountPrice = ruleProduct.Qty * discount.SingleDiscountPrice,
                                SingleDiscountPrice = discount.SingleDiscountPrice

                            });
                            usedSingleDiscount += ruleProduct.Qty * discount.SingleDiscountPrice;
                        }
                        endFlag++;
                    }
                    //}
                }


                return result;
           
        }

        private PromotionRuleDiscountView GetPromotionRule(Guid promoRuleId, OrderDetail orderDetail)
        {
            PromotionRuleDiscountView view = new PromotionRuleDiscountView();
            var rule = baseRepository.GetModelById<PromotionRule>(promoRuleId);
            if (rule != null)
            {
                decimal setQty = Math.Floor(orderDetail.Qty / rule.X);

                view.Id = rule.Id;
                view.DiscountPrice = Math.Round((setQty * rule.X * orderDetail.SalePrice) - (setQty * rule.Y), 2);
                view.SingleDiscountPrice = Math.Round(view.DiscountPrice / orderDetail.Qty, 2);
                view.ProductId = orderDetail.SkuId;
            }

            return view;
        }

        private decimal GetSetPrice(OrderDeliveryDetailDto detail, Guid ruleId)
        {
            decimal result = 0;
            var rule = baseRepository.GetModelById<PromotionRule>(ruleId);
            if (rule != null)
            {
                decimal setQty = Math.Floor(detail.Qty / rule.X);

                if (setQty >= 1)
                {
                    result = (setQty * rule.Y) + ((detail.Qty - (setQty * rule.X)) * detail.SalePrice);
                }
            }

            return result;
        }

        private void CalculateSubOrderDiscount(OrderDeliveryDto delivery, CheckoutItem checkOutItem)
        {
            if (checkOutItem.Discounts != null)
            {
                var discounts = checkOutItem.Discounts.OrderByDescending(o => o.DiscountType).ToList();

                var discountPrice = delivery.DiscountPrice;//此時傳入的值為原總價
                var discountFreight = delivery.DiscountFreight;//此時傳入的值為運費
                foreach (var item in discounts)
                {
                    switch (item.DiscountType)
                    {
                        case DiscountType.Coupon:
                            {
                                #region E-Coupon

                                var coupon = couponRepository.GetCouponById(item.Id);
                                item.IsPercent = coupon.IsPercent;
                                item.DiscountValue = coupon.DiscountValue;
                                item.DiscountPrice = CalculateSubOrderCoupon(delivery, coupon, discountPrice, discountFreight);//优惠的价钱
                                if (coupon.CouponType == CouponUsage.DeliveryCharge)
                                {
                                    delivery.DiscountFreight -= item.DiscountPrice;
                                }
                                else
                                {
                                    delivery.DiscountPrice -= item.DiscountPrice;
                                }

                                #endregion
                            }
                            break;
                        case DiscountType.PromotionCode:
                            var codeCoupon = baseRepository.GetModelById<PromotionCodeCoupon>(item.Id);
                            item.IsPercent = codeCoupon.IsDiscount;
                            item.DiscountValue = codeCoupon.DiscountAmount;
                            item.DiscountPrice = CalculateSubOrderPromotionCode(delivery, codeCoupon, discountPrice, discountFreight);//优惠的价钱
                            if (codeCoupon.CouponUsage == CouponUsage.DeliveryCharge)
                            {
                                delivery.DiscountFreight -= item.DiscountPrice;
                            }
                            else
                            {
                                delivery.DiscountPrice -= item.DiscountPrice;
                            }
                            break;
                        case DiscountType.MemberGroup:
                            var mgDiscount = baseRepository.GetModelById<MemberGroupDiscount>(item.Id);
                            item.IsPercent = mgDiscount.IsDiscount;
                            item.DiscountValue = mgDiscount.DiscountAmount;
                            item.DiscountPrice = CalculateSubOrderMemberGroupDiscount(delivery, mgDiscount);//优惠的价钱
                            break;
                        case DiscountType.PromotionRule:
                            delivery.DiscountPrice = delivery.DiscountPrice - item.DiscountPrice;
                            break;
                    }
                }
            }
        }

        private decimal CalculateSubOrderCoupon(OrderDeliveryDto delivery, DiscountInfo couponDiscount, decimal discountPrice, decimal discountFregiht)
        {
            decimal result = 0;
            if (couponDiscount != null)
            {
                switch (couponDiscount.CouponType)
                {
                    case CouponUsage.DeliveryCharge:
                        var beforeFreight = discountFregiht;
                        decimal freight = 0;
                        if (couponDiscount.IsPercent)
                        {
                            freight = Math.Round(discountFregiht - (discountFregiht * (couponDiscount.DiscountValue / 100)), 2);
                            //delivery.DiscountFreight = freight < 0 ? 0 : freight;
                        }
                        else
                        {
                            freight = Math.Round(discountFregiht - couponDiscount.DiscountValue, 2);
                            //delivery.DiscountFreight = freight < 0 ? 0 : freight;
                        }
                        result = beforeFreight - (freight < 0 ? 0 : freight);
                        break;
                    case CouponUsage.Price:
                        var beforePrice = discountPrice;
                        decimal price = 0;
                        if (couponDiscount.IsPercent)
                        {
                            price = Math.Round(discountPrice - (discountPrice * (couponDiscount.DiscountValue / 100)), 2);
                            //delivery.DiscountPrice = price < 0 ? 0 : price;
                        }
                        else
                        {
                            price = Math.Round(discountPrice - couponDiscount.DiscountValue, 2);
                            //delivery.DiscountPrice = price < 0 ? 0 : price;
                        }
                        result = beforePrice - (price < 0 ? 0 : price);
                        break;
                }
            }
            return result;

        }

        /// <summary>
        /// 計算分單的推廣優惠碼的優惠總額
        /// </summary>
        /// <param name="delivery">分單資料</param>
        /// <param name="codeCoupon">優惠券資料</param>
        /// <param name="discountPrice">參與計算的分單小計</param>
        /// <param name="discountFregiht">參與計算的分單運費</param>
        /// <returns></returns>
        private decimal CalculateSubOrderPromotionCode(OrderDeliveryDto delivery, PromotionCodeCoupon codeCoupon, decimal discountPrice, decimal discountFregiht)
        {
            decimal result = 0;
            if (codeCoupon != null)
            {
                switch (codeCoupon.CouponUsage)
                {
                    case CouponUsage.DeliveryCharge:
                        {
                            #region 運費優惠計算

                            var beforeFreight = discountFregiht;
                            decimal freight = 0;
                            if (codeCoupon.IsDiscount)
                            {
                                freight = Math.Round(discountFregiht - (discountFregiht * (codeCoupon.DiscountAmount / 100)), 2);
                                //delivery.DiscountFreight = freight < 0 ? 0 : freight;
                            }
                            else
                            {
                                freight = Math.Round(discountFregiht - codeCoupon.DiscountAmount, 2);
                                //delivery.DiscountFreight = freight < 0 ? 0 : freight;
                            }
                            result = beforeFreight - (freight < 0 ? 0 : freight);

                            #endregion
                        }
                        break;
                    case CouponUsage.Price:
                        {
                            decimal beforeAmt = discountPrice;
                            decimal price = 0;

                            #region 舊邏輯

                            //if (codeCoupon.IsDiscount)
                            //{
                            //    if (!codeCoupon.IsProdCoupon)//全品券，按總價直接參與計算
                            //    {
                            //        price = Math.Round(discountPrice - (discountPrice * (codeCoupon.DiscountAmount / 100)), 2);
                            //        //delivery.DiscountPrice = price < 0 ? 0 : price;
                            //    }
                            //    else//限品券，需要找出符合條件的產品價格參與計算
                            //    {
                            //        if (delivery.DeliveryDetails?.Count > 0 && codeCoupon.ProductList?.Count > 0)
                            //        {
                            //            decimal totalProdAmt = 0;
                            //            foreach (var deliveryDetail in delivery.DeliveryDetails)
                            //            {
                            //                string prodCode = _productSkuRepository.Find(deliveryDetail.SkuId)?.ProductCode ?? string.Empty;
                            //                if (codeCoupon.ProductList.Count(x => x.ProductCode == prodCode && x.IsActive && !x.IsDeleted) > 0)
                            //                {
                            //                    decimal salePrice = deliveryDetail.SalePrice;
                            //                    decimal setPrice = Math.Round(salePrice - (salePrice * (codeCoupon.DiscountAmount / 100)), 2);
                            //                    deliveryDetail.SetPrice = setPrice;
                            //                    totalProdAmt += deliveryDetail.SalePrice * deliveryDetail.Qty;
                            //                }
                            //            }
                            //            result += Math.Round(totalProdAmt * (codeCoupon.DiscountAmount / 100), 2);
                            //            return result;
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    price = Math.Round(discountPrice - codeCoupon.DiscountAmount, 2);
                            //    //delivery.DiscountPrice = price < 0 ? 0 : price;
                            //}

                            #endregion
                            decimal discountProdPrice = 0;
                            if (!codeCoupon.IsProdCoupon)
                            {
                                #region 全品類優惠

                                foreach (var deliveryDetail in delivery.DeliveryDetails)
                                {
                                    int totalQty = deliveryDetail.Qty;
                                    decimal unitPrice = deliveryDetail.SalePrice;
                                    int originalQty = totalQty - codeCoupon.MaxUsagePerProd;

                                    if (codeCoupon.MaxUsagePerProd > 0 && originalQty > 0)
                                    {
                                        #region 超出最大使用量，需要拆開保存結果

                                        discountProdPrice = CalculatePrmtCodeDiscountVal(unitPrice, codeCoupon.IsDiscount, codeCoupon.DiscountAmount);
                                        price += originalQty * unitPrice;
                                        price += codeCoupon.MaxUsagePerProd * discountProdPrice;

                                        #endregion
                                    }
                                    else
                                    {
                                        #region 商品全部在最大使用量範圍內

                                        discountProdPrice = CalculatePrmtCodeDiscountVal(unitPrice, codeCoupon.IsDiscount, codeCoupon.DiscountAmount);
                                        price += totalQty * discountProdPrice;

                                        #endregion
                                    }
                                    deliveryDetail.PayPrice = discountProdPrice;
                                }

                                #endregion
                            }
                            else
                            {
                                #region 限品類優惠

                                //可優惠的產品清單
                                var codeProdList =  baseRepository.GetList<PromotionCodeProduct>(x=>x.RuleId == codeCoupon.Id && x.IsActive && !x.IsDeleted).ToList();
                                if (codeProdList?.Count > 0)
                                {
                                    foreach (var deliveryDetail in delivery.DeliveryDetails)
                                    {
                                        string prodCode = baseRepository.GetModelById<Product>(deliveryDetail.ProductId)?.Code;
                                        decimal unitPrice = deliveryDetail.SalePrice;
                                        int totalQty = deliveryDetail.Qty;

                                        var validPrmtProdDetail = codeProdList.Where(x => x.ProductCode.ToUpper().Trim() == prodCode.ToUpper().Trim()).OrderByDescending(x => x.UpdateDate).FirstOrDefault();

                                        if (validPrmtProdDetail != null)
                                        {
                                            if (validPrmtProdDetail.HasCustDiscount)
                                            {
                                                //有設定產品的客製化優惠
                                                int originalQty = totalQty - validPrmtProdDetail.MaxUsage.Value;//超過最大使用量

                                                if (validPrmtProdDetail.MaxUsage.Value > 0 && originalQty > 0)
                                                {
                                                    discountProdPrice = CalculatePrmtCodeDiscountVal(unitPrice, validPrmtProdDetail.IsPercent.Value, validPrmtProdDetail.DiscountValue.Value);
                                                    price += originalQty * unitPrice;
                                                    price += validPrmtProdDetail.MaxUsage.Value * discountProdPrice;
                                                }
                                                else
                                                {
                                                    discountProdPrice = CalculatePrmtCodeDiscountVal(unitPrice, validPrmtProdDetail.IsPercent.Value, validPrmtProdDetail.DiscountValue.Value);
                                                    price += totalQty * discountProdPrice;
                                                }
                                            }
                                            else
                                            {
                                                //沒有客製化優惠，跟隨公用優惠
                                                int originalQty = totalQty - codeCoupon.MaxUsagePerProd;//超過最大使用量

                                                if (codeCoupon.MaxUsagePerProd > 0 && originalQty > 0)//限制優惠使用量
                                                {
                                                    discountProdPrice = CalculatePrmtCodeDiscountVal(unitPrice, codeCoupon.IsDiscount, codeCoupon.DiscountAmount);
                                                    price += originalQty * unitPrice;
                                                    price += codeCoupon.MaxUsagePerProd * discountProdPrice;
                                                }
                                                else
                                                {
                                                    discountProdPrice = CalculatePrmtCodeDiscountVal(unitPrice, codeCoupon.IsDiscount, codeCoupon.DiscountAmount);
                                                    price += totalQty * discountProdPrice;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //該產品沒有享受優惠
                                            discountProdPrice = unitPrice;
                                            price += totalQty * unitPrice;
                                        }
                                        deliveryDetail.PayPrice = discountProdPrice;
                                    }
                                }

                                #endregion
                            }

                            price = Math.Round(price, 2);
                            result = beforeAmt - (price < 0 ? 0 : price);
                        }
                        break;
                }
            }
            return result;
        }

        private decimal CalculateSubOrderMemberGroupDiscount(OrderDeliveryDto delivery, MemberGroupDiscount mgCoupon)
        {
            decimal result = 0;
            if (mgCoupon != null)
            {
                var beforePrice = delivery.DiscountPrice;
                if (mgCoupon.IsDiscount)
                {
                    delivery.DiscountPrice = Math.Round(delivery.DiscountPrice - (delivery.DiscountPrice * (mgCoupon.DiscountAmount / 100)), 2);
                }
                else
                {
                    delivery.DiscountPrice = Math.Round(delivery.DiscountPrice - mgCoupon.DiscountAmount, 2);
                }
                result = beforePrice - delivery.DiscountPrice;
            }
            return result;
        }

        /// <summary>
        /// 計算推廣優惠碼的優惠金額結果
        /// </summary>
        /// <param name="originalPrice">原價</param>
        /// <param name="isPercent">是否百分比值</param>
        /// <param name="discountVal">優惠額</param>
        private decimal CalculatePrmtCodeDiscountVal(decimal originalPrice, bool isPercent, decimal discountVal)
        {
            decimal resultVal = 0;
            if (isPercent)
            {
                originalPrice *= (100 - discountVal) / 100;
            }
            else
            {
                originalPrice -= discountVal;
            }

            resultVal = originalPrice;

            if (originalPrice < 0)
            {
                resultVal = 0;
            }

            return resultVal;
        }

        /// <summary>
        /// 創建訂單時，計算好冷靜期,如果商戶沒配置，取全局
        /// </summary>
        /// <param name="merchId"></param>
        /// <param name="coutryCode"></param>
        /// <returns></returns>
        private int GetCoolDownDate(Guid merchId, string coutryCode)
        {
            var promotion = merchantBLL.GetMerchPromotionInfo(merchId);
            if (promotion == null)
                return GetCalmeDate();

            //香港 
            if (coutryCode == "HKG")
                return promotion.LocalCoolDownDay <= 0 ? 7 : promotion.LocalCoolDownDay;                //本地
            else
                return promotion.OverSeaCoolDownDay <= 0 ? 7 : promotion.OverSeaCoolDownDay;     //海外
        }

        private int GetCalmeDate()
        {
            return settingBLL.GetOrderGracePeriodValue();
        }

        private void CalculateOrderDiscount(OrderDto order, NewOrder orderView)
        {
            if (orderView.Discounts != null)
            {
                var discounts = orderView.Discounts.OrderByDescending(o => o.DiscountType).ToList();
                var discountPrice = order.DiscountPrice;
                var discountFreight = order.DiscountFreight;
                foreach (var item in discounts)
                {
                    switch (item.DiscountType)
                    {
                        case DiscountType.Coupon:
                            var coupon = couponRepository.GetCouponById(item.Id);
                            item.IsPercent = coupon.IsPercent;
                            item.DiscountValue = coupon.DiscountValue;
                            item.DiscountPrice = CalculateOrderCoupon(order, coupon, discountPrice, discountFreight);
                            if (coupon.CouponType == CouponUsage.DeliveryCharge)
                            {
                                order.DiscountFreight -= item.DiscountPrice;
                            }
                            else
                            {
                                order.DiscountPrice -= item.DiscountPrice;
                            }
                            break;
                        case DiscountType.PromotionCode:
                            var codeCoupon = baseRepository.GetModelById<PromotionCodeCoupon>(item.Id);
                            item.IsPercent = codeCoupon.IsDiscount;
                            item.DiscountValue = codeCoupon.DiscountAmount;
                            item.DiscountPrice = CalculateOrderPromotionCode(order, codeCoupon, discountPrice, discountFreight);
                            if (codeCoupon.CouponUsage == CouponUsage.DeliveryCharge)
                            {
                                order.DiscountFreight -= item.DiscountPrice;
                            }
                            else
                            {
                                order.DiscountPrice -= item.DiscountPrice;
                            }
                            break;
                        case DiscountType.MemberGroup:
                            var mgDiscount = baseRepository.GetModelById<MemberGroupDiscount>(item.Id);
                            CalculateOrderMemberGroupDiscount(order, mgDiscount);
                            break;
                    }
                }
            }
        }

        private decimal CalculateOrderCoupon(OrderDto order, DiscountInfo couponDiscount, decimal discountPrice, decimal discountFreight)
        {
            decimal result = 0;
            if (couponDiscount != null)
            {
                switch (couponDiscount.CouponType)
                {
                    case CouponUsage.DeliveryCharge:
                        var beforeFreight = discountFreight;
                        decimal freight = 0;
                        if (couponDiscount.IsPercent)
                        {
                            freight = Math.Round(discountFreight - (discountFreight * (couponDiscount.DiscountValue / 100)), 2);
                            //order.DiscountFreight = freight < 0 ? 0 : freight;
                            result = freight < 0 ? beforeFreight : (discountFreight * (couponDiscount.DiscountValue / 100));
                        }
                        else
                        {
                            freight = Math.Round(discountFreight - couponDiscount.DiscountValue, 2);
                            //order.DiscountFreight = freight < 0 ? 0 : freight;
                            result = freight > 0 ? beforeFreight : couponDiscount.DiscountValue;
                        }

                        break;
                    case CouponUsage.Price:
                        var beforePrice = discountPrice;
                        decimal price = 0;
                        if (couponDiscount.IsPercent)
                        {
                            price = Math.Round(discountPrice - (discountPrice * (couponDiscount.DiscountValue / 100)), 2);
                            //order.DiscountPrice = price < 0 ? 0 : price;
                            result = price < 0 ? discountPrice : (discountPrice * (couponDiscount.DiscountValue / 100));
                        }
                        else
                        {
                            price = Math.Round(discountPrice - couponDiscount.DiscountValue, 2);
                            //order.DiscountPrice = price < 0 ? 0 : price;
                            result = price < 0 ? beforePrice : couponDiscount.DiscountValue;
                        }

                        break;
                }
            }
            return result;
        }

        private decimal CalculateOrderPromotionCode(OrderDto order, PromotionCodeCoupon codeCoupon, decimal discountPrice, decimal discountFreight)
        {
            decimal result = 0;
            if (codeCoupon != null)
            {
                switch (codeCoupon.CouponUsage)
                {
                    case CouponUsage.DeliveryCharge:
                        var beforeFreight = discountFreight;
                        decimal freight = 0;
                        if (codeCoupon.IsDiscount)
                        {
                            freight = Math.Round(discountFreight - (discountFreight * (codeCoupon.DiscountAmount / 100)), 2);
                            //order.DiscountFreight = freight < 0 ? 0 : freight;
                            result = freight < 0 ? beforeFreight : (discountFreight * (codeCoupon.DiscountAmount / 100));
                        }
                        else
                        {
                            freight = Math.Round(discountFreight - codeCoupon.DiscountAmount, 2);
                            //order.DiscountFreight = freight < 0 ? 0 : freight;
                            result = freight < 0 ? beforeFreight : codeCoupon.DiscountAmount;
                        }

                        break;
                    case CouponUsage.Price:
                        var beforePrice = discountPrice;
                        decimal price = 0;
                        if (codeCoupon.IsDiscount)
                        {
                            price = Math.Round(discountPrice - (discountPrice * (codeCoupon.DiscountAmount / 100)), 2);
                            //order.DiscountPrice = price < 0 ? 0 : price;
                            result = price < 0 ? beforePrice : (discountPrice * (codeCoupon.DiscountAmount / 100));
                        }
                        else
                        {
                            price = Math.Round(order.DiscountPrice - codeCoupon.DiscountAmount, 2);
                            //order.DiscountPrice = price < 0 ? 0 : price;
                            result = price < 0 ? beforePrice : codeCoupon.DiscountAmount;
                        }

                        break;
                }
            }
            return result;
        }

        private decimal CalculateOrderMemberGroupDiscount(OrderDto order, MemberGroupDiscount mgCoupon)
        {
            decimal result = 0;
            if (mgCoupon != null)
            {
                if (mgCoupon.IsDiscount)
                {
                    order.DiscountPrice = Math.Round(order.DiscountPrice - (order.DiscountPrice * (mgCoupon.DiscountAmount / 100)), 2);
                }
                else
                {
                    order.DiscountPrice = Math.Round(order.DiscountPrice - mgCoupon.DiscountAmount, 2);
                }
                result = order.DiscountPrice;
            }
            return result;
        }

        /// <summary>
        /// 更新会员积分
        /// </summary>
        /// <param name="mallFun"></param>
        private void UpdateMallFun(decimal mallFun)
        {
            if (mallFun > 0)
            {
                var memberAccount = baseRepository.GetModel<MemberAccount>(x=>x.MemberId==Guid.Parse(CurrentUser.UserId) && x.IsActive && !x.IsDeleted);
                if (memberAccount != null)
                {
                    memberAccount.Fun -= mallFun;

                    FunHistory history = new FunHistory();
                    history.Id = Guid.NewGuid();
                    history.AccountId = memberAccount.Id;
                    history.Amount = mallFun;
                    history.Type = InOut.Out;
                    
                    baseRepository.Insert(history);
                    baseRepository.Update(memberAccount);
                }
            }
        }

        private void InsertOrderStatusHistory(Guid id, OrderStatus status)
        {
            var operatorDate = DateTime.Now;

            var lastHistory = baseRepository.GetModel<SubOrderStatusHistory>(p => p.IsActive && !p.IsDeleted && p.OrderId == id && p.Status == status-1);
            if (lastHistory != null)
            {
                lastHistory.UpdateBy = Guid.Parse(CurrentUser.UserId);
                lastHistory.UpdateDate = operatorDate;
                baseRepository.Update(lastHistory);
            }

            OrderStatusHistory orderStatushistory = new OrderStatusHistory();
            orderStatushistory.Id = Guid.NewGuid();

            orderStatushistory.CreateBy = Guid.Parse(CurrentUser.UserId);
            orderStatushistory.CreateDate = operatorDate;
            //orderStatushistory.Operator = UnitOfWork.Operator.Name;
            orderStatushistory.OrderId = id;
            orderStatushistory.Status = status;
            baseRepository.Insert(orderStatushistory);
        }

        private void InsertOrderDiscountRecord(List<DiscountView> discounts, Guid orderId)
        {
            List<OrderDiscount> discountList = new List<OrderDiscount>();
            List<Coupon> couponList = new List<Coupon>();
            List<PromotionCodeCoupon> promotionCodeCoupons = new List<PromotionCodeCoupon>();
            if (discounts != null)
            {
                foreach (var item in discounts)
                {
                    discountList.Add(new OrderDiscount
                    {
                        Id = Guid.NewGuid(),
                        DiscountId = item.Id,
                        DiscountPrice = item.DiscountPrice,
                        DiscountType = item.DiscountType,
                        DiscountUsage = item.CouponType,
                        DiscountValue = item.DiscountValue,
                        IsPercent = item.IsPercent,
                        OrderId = orderId,
                        SubOrderId = Guid.Empty,
                        ProductId = item.ProductId,
                        MemberId = Guid.Parse(CurrentUser.UserId),
                        Code = item?.Code ?? ""
                    });

                    switch (item.DiscountType)
                    {
                        case DiscountType.Coupon:
                            //更新coupon的使用日期、使用訂單、子訂單
                            var coupon = baseRepository.GetModelById<Coupon>(item.Id);
                            if (coupon != null)
                            {
                                coupon.OrderId = orderId;
                                coupon.DeliveryId = Guid.Empty;
                                coupon.UseDate = DateTime.Now;
                                couponList.Add(coupon);
                            }
                            break;
                        case DiscountType.PromotionCode:
                            var codeCoupon = baseRepository.GetModelById<PromotionCodeCoupon>(item.Id);
                            if (codeCoupon != null)
                            {
                                codeCoupon.UseCount += 1;
                                promotionCodeCoupons.Add(codeCoupon);

                            }

                            break;
                            //case DiscountType.MemberGroup:

                            //    break;
                            //case DiscountType.PromotionRule:

                            //    break;
                    }


                }

                baseRepository.Update(couponList);
                baseRepository.Update(promotionCodeCoupons);           
                baseRepository.Insert(discountList);
            }

        }

        private void InsertSubOrderStatusHistory(Guid id, Guid subOrderId, OrderStatus status)
        {
            var operatorDate = DateTime.Now;

            var lastHistory = baseRepository.GetModel<SubOrderStatusHistory>(p => p.IsActive && !p.IsDeleted && p.OrderId == id && p.SubOrderId == subOrderId && p.Status == status - 1);
            if (lastHistory != null)
            {
                lastHistory.UpdateBy = Guid.Parse(CurrentUser.UserId);
                lastHistory.UpdateDate = operatorDate;
                baseRepository.Update(lastHistory);
            }
            
            SubOrderStatusHistory subOrderStatushistory = new SubOrderStatusHistory();
            subOrderStatushistory.Id = Guid.NewGuid();

            subOrderStatushistory.CreateBy = Guid.Parse(CurrentUser.UserId);
            subOrderStatushistory.CreateDate = DateTime.Now;
            //subOrderStatushistory.Operator = UnitOfWork.Operator.Name;
            subOrderStatushistory.OrderId = id;
            subOrderStatushistory.SubOrderId = subOrderId;
            subOrderStatushistory.Status = status;
            baseRepository.Insert(subOrderStatushistory);
        }

        private void InsertSubOrderDiscountRecord(List<DiscountView> discounts, Guid orderId, Guid subOrderId)
        {
            List<OrderDiscount> discountList = new List<OrderDiscount>();
            List<Coupon> couponList = new List<Coupon>();
            List<PromotionCodeCoupon> promotionCodeCoupons = new List<PromotionCodeCoupon>();
            if (discounts != null)
            {
                foreach (var item in discounts)
                {
                    discountList.Add(new OrderDiscount
                    {
                        Id = Guid.NewGuid(),
                        DiscountId = item.Id,
                        DiscountPrice = item.DiscountPrice,
                        DiscountType = item.DiscountType,
                        DiscountUsage = item.CouponType,
                        DiscountValue = item.DiscountValue,
                        IsPercent = item.IsPercent,
                        OrderId = orderId,
                        SubOrderId = subOrderId,
                        ProductId = item.ProductId,
                        MemberId = Guid.Parse(CurrentUser.UserId),
                        Code = item?.Code ?? ""

                    });

                    switch (item.DiscountType)
                    {
                        case DiscountType.Coupon:
                            //更新coupon的使用日期、使用訂單、子訂單
                            var coupon = baseRepository.GetModelById<Coupon>(item.Id);
                            if (coupon != null)
                            {
                                coupon.OrderId = orderId;
                                coupon.DeliveryId = subOrderId;
                                coupon.UseDate = DateTime.Now;
                                couponList.Add(coupon);
                            }
                            break;
                        case DiscountType.PromotionCode:
                            var codeCoupon = baseRepository.GetModelById<PromotionCodeCoupon>(item.Id);
                            if (codeCoupon != null)
                            {
                                codeCoupon.UseCount += 1;
                                promotionCodeCoupons.Add(codeCoupon);
                            }

                            break;
                            //case DiscountType.PromotionRule:
                            //    break;
                            //case DiscountType.MemberGroup:
                            //    break;
                    }
                }               
                baseRepository.Update(couponList);
                baseRepository.Update(promotionCodeCoupons);
                baseRepository.Insert(discountList);
            }

        }

        private bool CheckDeliveryStautsIsSame(UpdateStatusCondition cond)
        {
            bool result = true;
            if (cond.DeliveryTrackingInfo.Any())
            {
                var id = cond.DeliveryTrackingInfo[0].Id;
                var orderDelivery = baseRepository.GetModelById<OrderDelivery>(id);
                if (orderDelivery == null) return false;
                if (orderDelivery.Status != cond.CurrentStatus) return false;
            }

            return result;
        }
    }
}
