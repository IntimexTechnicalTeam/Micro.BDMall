namespace BDMall.Model
{
    /// <summary>
    /// 商品目录
    /// </summary>
    public class ProductCatalog : BaseEntity<Guid>
    {
        /// <summary>
        /// 多语言Id
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid NameTransId { get; set; }

        /// <summary>
        /// 父目录ID
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid ParentId { get; set; }


        /// <summary>
        /// 目录图标
        /// </summary>  

        [Column(Order = 5, TypeName = "varchar")]
        [StringLength(200)]
        public string SmallIcon { get; set; }

        [Column(Order = 6, TypeName = "varchar")]
        [StringLength(200)]
        public string BigIcon { get; set; }

        [Column(Order = 7, TypeName = "varchar")]
        [StringLength(200)]
        public string OriginalIcon { get; set; }
        /// <summary>
        /// 目录手机图标
        /// </summary>  

        [Column(Order = 12, TypeName = "varchar")]
        [StringLength(200)]
        public string MSmallIcon { get; set; }

        [Column(Order = 13, TypeName = "varchar")]
        [StringLength(200)]
        public string MBigIcon { get; set; }

        [Column(Order = 14, TypeName = "varchar")]
        [StringLength(200)]
        public string MOriginalIcon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column(Order = 8)]
        public int Seq { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [Column(Order = 9)]
        public int Level { get; set; }

        [Column(Order = 10, TypeName = "varchar")]
        [StringLength(50)]
        public string Code { get; set; }

    }
}
