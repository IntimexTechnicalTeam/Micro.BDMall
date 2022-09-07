namespace BDMall.Model
{
    public class MerchantFreeCharge : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid MerchantId { get; set; }

        [StringLength(100)]
        [Column(Order = 4, TypeName = "varchar")]
        public string ProductCode { get; set; }

        [StringLength(10)]
        [Column(Order = 5, TypeName = "varchar")]
        public string ShipCode { get; set; }


    }
}
