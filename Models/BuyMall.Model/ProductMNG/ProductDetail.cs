namespace BDMall.Model
{
    /// <summary>
    /// 產品詳細描述的翻譯表
    /// </summary>
    public class ProductDetail : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid ProductId { get; set; }
        [Column(Order = 4)]
        public Guid TransId { get; set; }
        [Column(Order = 5)]
        public Language Lang { get; set; }
        [Column(Order = 6)]
        public string Value { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }





    }
}
