namespace BDMall.Domain
{
    public class OrderShowCond
    {
        /// <summary>
        /// 取多少条
        /// </summary>
        public int TopQty { get; set; } = 0;

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; } = OrderStatus.ReceivedOrder;

        /// <summary>
        /// 这个作为字典的Key,提供给前端使用，如newReceivedOrderLst，completedOrderLst，这些是前端的参数标识
        /// </summary>
        public string TableType { get; set; }

        public List<OrderShowCond> OrderCondList { get; set; } = new List<OrderShowCond>();
    }

}
