namespace BDMall.Domain
{
    public class OrderCondition
    {
        public string PayRef { get; set; } = "";
        /// <summary>
        /// 訂單生日日期開始日期
        /// </summary>
        public string CreateDateFrom { get; set; } = "";
        /// <summary>
        /// 訂單生日日期結束日期
        /// </summary>
        public string CreateDateTo { get; set; } = "";

        /// <summary>
        /// 發票編號
        /// </summary>
        public string InvoiceNoFrom { get; set; } = "";

        /// <summary>
        /// 發票編號
        /// </summary>
        public string InvoiceNoTo { get; set; } = "";

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderNo { get; set; } = "";


        /// <summary>
        /// 送貨單編號
        /// </summary>
        public string DeliveryNoFrom { get; set; } = "";

        /// <summary>
        /// /// 送貨單編號
        /// </summary>
        public string DeliveryNoTo { get; set; } = "";


        /// <summary>
        /// 送貨方式
        /// </summary>
        public Guid ExpressId { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public Guid PaymentMethod { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public OrderStatus StatusCode { get; set; }

        /// <summary>
        /// 用于查询旧订单的状态
        /// </summary>
        public string StatusName { get; set; } = "";

        public PageInfo PageInfo { get; set; }

        public Guid MerchantId { get; set; }
        public Guid MemberId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string OrderBy { get; set; } = "";

        /// <summary>
        /// 是否前端搜寻
        /// </summary>
        public bool IsFront { get; set; }

        /// <summary>
        /// 送货单的投寄易状态
        /// </summary>
        public int ECShipStatus { get; set; }

        public int EventReward { get; set; }

        /// <summary>
        /// 订单折扣
        /// </summary>
        public string OrderDiscount { get; set; } = "";
    }
}
