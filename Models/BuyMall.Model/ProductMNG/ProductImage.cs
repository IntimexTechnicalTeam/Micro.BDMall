namespace BDMall.Model
{
    public class ProductImage : BaseEntity<Guid>
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        [Column(Order = 3)]
        public Guid ProductId { get; set; }

        /// <summary>
        /// 屬性值1
        /// </summary>
        [Column(Order = 4)]
        public Guid AttrValue1 { get; set; }

        /// <summary>
        /// 屬性值2
        /// </summary>
        [Column(Order = 5)]
        public Guid AttrValue2 { get; set; }

        /// <summary>
        /// 屬性值3
        /// </summary>
        [Column(Order = 6)]
        public Guid AttrValue3 { get; set; }


        /// <summary>
        /// 圖片的類型,屬性圖片、附加圖片
        /// </summary>
        [Column(Order = 7)]
        public ImageType Type { get; set; }
        /// <summary>
        /// 图片的正反面
        /// </summary>
        [Column(Order = 8)]
        public ImageSide Side { get; set; }

        public virtual ICollection<ProductImageList> ImageItems { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
