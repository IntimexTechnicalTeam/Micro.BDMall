namespace BDMall.Model
{
    public class MerchantPromotionProduct : BaseEntity<Guid>
    {
        [Required]
        [Column(Order = 4)]
        public Guid PromotionId { get; set; }


        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 5)]
        public string ProductCode { get; set; }

        [Column(Order = 6)]
        public Guid ProductId { get; set; }

        [ForeignKey("PromotionId")]
        public virtual MerchantPromotion MerchantPromotion { get; set; }
    }
}
