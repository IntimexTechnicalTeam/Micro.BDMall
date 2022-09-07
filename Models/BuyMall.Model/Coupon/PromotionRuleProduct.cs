namespace BDMall.Model
{
    public class PromotionRuleProduct : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid PromotionRuleId { get; set; }

        [Column(Order = 4)]
        public Guid ProductId { get; set; }


        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 5)]
        public string ProductCode { get; set; }

        [ForeignKey("PromotionRuleId")]
        public virtual PromotionRule PromotionRule { get; set; }
    }
}
