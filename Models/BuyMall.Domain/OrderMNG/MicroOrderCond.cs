namespace BDMall.Domain
{
    public class MicroOrderCond
    {
        /// <summary>
        /// 訂單狀態
        /// </summary>
        public OrderStatus StatusCode { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}
