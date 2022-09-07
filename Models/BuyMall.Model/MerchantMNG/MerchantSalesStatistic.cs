namespace BDMall.Model
{
    /// <summary>
    /// 商家銷售數據統計
    /// </summary>
    public class MerchantSalesStatistic : BaseEntity<int>
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid MerchantId { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public int Qty { get; set; }
    }
}
