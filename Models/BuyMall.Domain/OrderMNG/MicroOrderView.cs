namespace BDMall.Domain
{
    public class MicroOrderView
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNO { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        ///  订单合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 數量合計
        /// </summary>
        public int ItemQty { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
