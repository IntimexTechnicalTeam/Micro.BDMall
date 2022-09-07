namespace BDMall.Domain
{
    public class OrderStatusGroup
    {
        /// <summary>
        /// 收到訂單
        /// </summary>
        public KeyValue RecevedOrder { get; set; }
        /// <summary>
        /// 己確認付款
        /// </summary>
        public KeyValue PaymentConfirmed { get; set; }
        /// <summary>
        /// 處理中
        /// </summary>
        public KeyValue Processing { get; set; }

        /// <summary>
        /// 已安排送貨
        /// </summary>
        public KeyValue DeliveryArranged { get; set; }

        /// <summary>
        /// 訂單完成
        /// </summary>
        public KeyValue OrderCompleted { get; set; }
        /// <summary>
        /// 訂單取消
        /// </summary>
        public KeyValue OrderCancelled { get; set; }


    }
}
