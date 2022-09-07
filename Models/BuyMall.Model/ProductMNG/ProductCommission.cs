namespace BDMall.Model
{
    /// <summary>
    /// 產品佣金
    /// </summary>
    public class ProductCommission : BaseEntity<Guid>
    {
        /// <summary>
        /// 關聯的產品ID
        /// </summary>
        [Column(Order = 3)]
        public Guid ProductId { get; set; }
        /// <summary>
        /// 佣金額
        /// </summary>
        [Column(Order = 4)]
        public decimal? CMVal { get; set; }
        /// <summary>
        /// 佣金率
        /// </summary>
        [Column(Order = 5)]
        public decimal? CMRate { get; set; }
        /// <summary>
        /// 計算類型（1、產品自訂 2、跟隨商戶設定 3、成本價格差）
        /// </summary>
        [Column(Order = 6)]
        public ProdCommissionType CMCalType { get; set; }
    }
}
