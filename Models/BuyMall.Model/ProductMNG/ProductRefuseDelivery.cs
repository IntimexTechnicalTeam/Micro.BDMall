namespace BDMall.Model
{
    public class ProductRefuseDelivery : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid ProductId { get; set; }

        public int CountryId { get; set; }


        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}

