namespace BDMall.Model
{
    /// <summary>
    /// 产品SKU
    /// </summary>
    public class ProductSku : BaseEntity<Guid>
    {

        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 3)]
        public string ProductCode { get; set; }
        [Column(Order = 4)]
        public Guid Attr1 { get; set; }
        [Column(Order = 5)]
        public Guid Attr2 { get; set; }
        [Column(Order = 6)]
        public Guid Attr3 { get; set; }
        [Column(Order = 7)]
        public Guid AttrValue1 { get; set; }
        [Column(Order = 8)]
        public Guid AttrValue2 { get; set; }
        [Column(Order = 9)]
        public Guid AttrValue3 { get; set; }

       






    }
}
