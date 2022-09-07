namespace BDMall.Model
{
    public class ProductStatistics : BaseEntity<Guid>
    {
        /// <summary>
        /// 產品編號
        /// </summary>
        [Column(Order = 3, TypeName = "varchar")]
        [StringLength(100)]
        public string Code { get; set; }
        [Column(Order = 4)]
        public decimal Score { get; set; }

        [Column(Order = 5)]
        public int ScoreNum { get; set; }

        /// <summary>
        /// 购买次数
        /// </summary>
        [Column(Order = 6)]
        public int PurchaseCounter { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        [Column(Order = 7)]
        public int VisitCounter { get; set; }

        /// <summary>
        /// 總分
        /// </summary>
        [Column(Order = 8)]
        public decimal TotalScore { get; set; }

        [Column(Order = 9)]
        public Guid InternalNameTransId { get; set; }

        /// <summary>
        /// 搜索次数
        /// </summary>
        [Column(Order = 10)]
        public int SearchCounter { get; set; }

        /// <summary>
        /// 所屬商家ID
        /// </summary>
        [Required]
        [Column(Order = 11)]
        public Guid MerchantId { get; set; }
    }
}
