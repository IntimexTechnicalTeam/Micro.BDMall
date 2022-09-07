namespace BDMall.Model
{
    public class ProductCatalogAttr : BaseEntity<Guid>
    {

        [Column(Order = 3)]
        public Guid CatalogId { get; set; }
        [Column(Order = 4)]
        public Guid AttrId { get; set; }
        [Column(Order = 5)]
        public int Seq { get; set; }

        [Column(Order = 6)]
        /// <summary>
        /// 是否库存属性
        /// </summary>
        public bool IsInvAttribute { get; set; }
        //public virtual ICollection<ProductAttribute> Attributes { get; set; }
    }
}
