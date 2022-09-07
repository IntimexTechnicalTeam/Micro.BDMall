namespace BDMall.Model
{
    /// <summary>
    /// 订单时段价格改变明细
    /// </summary>
    public class OrderPriceDetail : BaseEntity<Guid>
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid OrderId
        {
            get; set;
        }

        /// <summary>
        /// 单品ID
        /// </summary>
        public Guid SkuId
        {
            get; set;
        }
        /// <summary>
        /// 產品Id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 生效开始日期
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 生效结束日期
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 购买价格
        /// </summary>
        public decimal SalePrice
        {
            get; set;
        }

        /// <summary>
        /// 时段价格
        /// </summary>
        public decimal TimePrice
        {
            get; set;
        }






    }
}
