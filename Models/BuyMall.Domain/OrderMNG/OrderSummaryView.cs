namespace BDMall.Domain
{
    public class OrderSummaryView
    {
        /// <summary>
        /// 訂單ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 訂單的產品列表
        /// </summary>
        public List<ProductSummary> Products { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// 訂單狀態名稱
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 訂單總價
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 折扣后的訂單總價
        /// </summary>
        public decimal DiscountTotalAmount { get; set; }

        /// <summary>
        /// 訂單產品總數
        /// </summary>
        public int TotalQty { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// 支付方式編碼
        /// </summary>
        public string PMCode { get; set; }

        /// <summary>
        /// 支付方式手續費率
        /// </summary>
        public decimal? PMRate { get; set; }

        public Guid PaymentMethodId { get; set; }

        /// <summary>
        /// 是否支付
        /// </summary>
        public bool IsPay { get; set; }

        /// <summary>
        /// 總重
        /// </summary>
        public decimal TotalWeight { get; set; }

        public SimpleCurrency Currency { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreateDateString { get; set; }

        public string UpdateDateString { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string MemberName { get; set; }
    }
}
