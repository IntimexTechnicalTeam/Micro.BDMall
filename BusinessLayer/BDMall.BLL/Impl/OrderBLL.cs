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

namespace BDMall.BLL
{
    public class OrderBLL : BaseBLL, IOrderBLL
    {
        public IOrderRepository orderRepository;
        public ICodeMasterRepository codeMasterRepository;
        public ICurrencyBLL currencyBLL;
        public IPaymentBLL paymentBLL;
        public IProductBLL productBLL;

        public OrderBLL(IServiceProvider services) : base(services)
        {
            orderRepository = Services.Resolve<IOrderRepository>();
            codeMasterRepository = Services.Resolve<ICodeMasterRepository>();
            currencyBLL = Services.Resolve<ICurrencyBLL>();
            paymentBLL= Services.Resolve<IPaymentBLL>();
            productBLL = Services.Resolve<IProductBLL>();   
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
    }
}
