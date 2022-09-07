namespace BDMall.Model
{
    public class PromotionRule : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid MerchantId { get; set; }

        [Column(Order = 4)]
        public PromotionRuleType Type { get; set; }

        [Column(Order = 5)]
        public decimal X { get; set; }

        [Column(Order = 6)]
        public decimal Y { get; set; }

        [Column(Order = 7)]
        public Guid RemarkTransId { get; set; }

        [Column(Order = 8)]
        public Guid TitleTransId { get; set; }

        [Column(Order = 9)]
        public DateTime EffectDateFrom { get; set; }

        [Column(Order = 10)]
        public DateTime EffectDateTo { get; set; }

        public virtual ICollection<PromotionRuleProduct> PromotionRuleProducts { get; set; }

    }
}
