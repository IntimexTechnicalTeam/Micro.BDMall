namespace BDMall.Model
{
    public class ProductSalesSummry : BaseEntity<int>
    {
        [Required]
        [Column(Order = 3)]
        public Guid MerchantId { get; set; }
        [Required]
        [Column(Order = 4)]
        public Guid Sku { get; set; }
        [Required]
        [Column(Order = 5)]
        public int Year { get; set; }
        [Required]
        [Column(Order = 6)]
        public int Month { get; set; }
        [Required]
        [Column(Order = 7)]
        public int Day { get; set; }
        [Required]
        [Column(Order = 8)]
        public int Qty { get; set; }
    }
}
