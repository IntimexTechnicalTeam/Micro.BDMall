namespace BDMall.Model
{
    public class ProductSpecification : BaseEntity<Guid>
    {

        [Key]
        [ForeignKey("Product")]
        public new Guid Id { get; set; }
    
        [Column(Order = 3)]
        public decimal Width { get; set; } = 0;

        [Column(Order = 4)]
        public decimal Heigth { get; set; } = 0;

        [Column(Order = 5)]
        public decimal Length { get; set; } = 0;

        [Column(Order = 6)]
        public int Unit { get; set; } = -1;

        [Column(Order = 7)]
        public decimal PackageWidth { get; set; } = 0;

        [Column(Order = 8)]
        public decimal PackageHeigth { get; set; } = 0;

        [Column(Order = 9)]
        public decimal PackageLength { get; set; } = 0;

        [Column(Order = 10)]
        public int PackageUnit { get; set; } = -1;


        /// <summary>
        /// 包裝描述
        /// </summary>
        [Column(Order = 11)]
        [StringLength(1000)]
        public string PackageDescription { get; set; }

        /// <summary>
        /// 毛重
        /// </summary>
        [Column(Order = 12)]
        public decimal GrossWeight { get; set; }


        /// <summary>
        /// 淨重
        /// </summary>
        [Column(Order = 13)]
        public decimal NetWeight { get; set; }

        /// <summary>
        /// 重量單位
        /// </summary>
        [Column(Order = 14)]
        public WeightUnit WeightUnit { get; set; }


        public virtual Product Product { get; set; }
    }
}
