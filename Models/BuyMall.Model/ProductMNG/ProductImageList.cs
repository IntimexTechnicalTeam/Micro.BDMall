namespace BDMall.Model
{
    public class ProductImageList : BaseEntity<Guid>
    {
        /// <summary>
        /// 產品圖片ID
        /// </summary>
        [Column(Order = 3)]
        public Guid ImageID { get; set; }

        /// <summary>
        /// 原圖路徑
        /// </summary>
        [Column(Order = 4, TypeName = "varchar")]
        [StringLength(200)]
        public string OriginalPath { get; set; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        [Column(Order = 5, TypeName = "varchar")]
        [StringLength(200)]
        public string Path { get; set; }
        [Column(Order = 6)]
        public long Size { get; set; }
        [Column(Order = 7, TypeName = "varchar")]
        [StringLength(10)]
        public string Width { get; set; }

        [Column(Order = 8, TypeName = "varchar")]
        [StringLength(10)]
        public string Length { get; set; }

        /// <summary>
        /// 圖片尺寸類型
        /// </summary>
        [Column(Order = 9)]
        public ImageSizeType Type { get; set; }


        [ForeignKey("ImageID")]
        public virtual ProductImage Image { get; set; }
    }
}
