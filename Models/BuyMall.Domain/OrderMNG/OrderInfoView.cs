namespace BDMall.Domain
{
    public class OrderInfoView
    {
        public string OrderId { get; set; }

        public string OrderNO { get; set; }
        public string InvoiceNO { get; set; }

        public Guid MemberId { get; set; }

        public SimpleMember Member { get; set; }

        public Guid PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        /// <summary>
        /// 支付方式服務費率
        /// </summary>
        public decimal? PMServRate { get; set; }
        /// <summary>
        /// PaymentMethodCode
        /// </summary>
        public string PMCode { get; set; }

        // public string CurrencyId { get; set; }

        public string OrderUrl { get; set; }

        public SimpleCurrency Currency { get; set; }

        public decimal ExchangerRate { get; set; }

        public decimal DeliveryCharge { get; set; }

        public decimal DeliveryDiscount { get; set; }
        //public DateTime? DeliveryDate { get; set; }

        public decimal ItemsAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal MallFun { get; set; }
        /// <summary>
        /// 折扣后的订单总价
        /// </summary>
        public decimal DiscountTotalAmount { get; set; }

        /// <summary>
        /// 折扣后的货物总价
        /// </summary>
        public decimal DiscountTotalPrice { get; set; }

        /// <summary>
        ///  优惠券、Promotion code优惠后的运费
        /// </summary>
        public decimal DiscountDeliveryCharge { get; set; }


        public decimal TotalWeight { get; set; }
        public double TotalQty { get; set; }



        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string UpdateDate { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// 送货单信息
        /// </summary>
        public List<OrderDeliveryInfo> Deliveries { get; set; }

        public BillInfo BillInfo { get; set; }


        public string StatusCode { get; set; }

        public string StatusName { get; set; }

        /// <summary>
        /// 買家是否付款
        /// </summary>
        public bool IsPay { get; set; }

        public Guid PaymentKey { get; set; }

        public bool IsMerchant { get; set; }

        public List<CouponDiscountInfo> Coupon { get; set; }

        /// <summary>
        /// 訂單使用的優惠券
        /// </summary>
        public List<DiscountView> Discounts { get; set; }

        public string EventRewardTips { get; set; }
    }
}
