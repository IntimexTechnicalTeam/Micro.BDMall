namespace BDMall.Domain
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public Guid SkuId { get; set; }
        public Guid MerchantId { get; set; }

        public ProductSummary Product { get; set; }

        public ProductSkuDto SkuInfo { get; set; }

        public int Qty { get; set; }

        public decimal UnitPrice { get; set; }

        public string TrackingNo { get; set; }

        /// <summary>
        /// 是否已申請退貨
        /// </summary>
        public bool HasReturned { get; set; }

        public string ReturnOrderId { get; set; }

        /// <summary>
        /// 所屬送貨單ID
        /// </summary>
        public Guid DeliveryId { get; set; }

        public Guid OrderId { get; set; }

        /// <summary>
        /// 所屬送貨單編號
        /// </summary>
        public string DeliveryNo { get; set; }

        public bool IsFree { get; set; }
    }
}
