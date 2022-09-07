namespace BDMall.Model
{
    public class PromotionCodeProduct : BaseEntity<Guid>
    {
        /// <summary>
        /// 優惠券ID
        /// </summary>
        [Column(Order = 3)]
        public Guid RuleId { get; set; }

        /// <summary>
        /// 產品編號
        /// </summary>
        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 4)]
        public string ProductCode { get; set; }

        /// <summary>
        /// 是否擁有自定義的優惠
        /// </summary>
        [Column(Order = 5)]
        public bool HasCustDiscount { get; set; }

        /// <summary>
        /// 是否以百分比計算
        /// </summary>
        [Column(Order = 6)]
        public bool? IsPercent { get; set; }

        /// <summary>
        /// 滿足條件的金額
        /// </summary>
        [Column(Order = 7)]
        public decimal? DiscountRange { get; set; }

        /// <summary>
        /// 優惠金額/百分比值
        /// </summary>
        [Column(Order = 8)]
        public decimal? DiscountValue { get; set; }

        /// <summary>
        /// 最大可優惠數量
        /// </summary>
        [Column(Order = 9)]
        public int? MaxUsage { get; set; }

        [ForeignKey("RuleId")]
        public virtual PromotionCodeCoupon PromotionCode { get; set; }
    }
}
