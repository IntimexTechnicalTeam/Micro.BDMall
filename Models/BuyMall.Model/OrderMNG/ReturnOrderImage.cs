namespace BDMall.Model
{
    /// <summary>
    /// 退換單圖片
    /// </summary>
    public class ReturnOrderImage : BaseEntity<Guid>
    {
        /// <summary>
        /// 退換單詳細ID
        /// </summary>
        [Required]
        [Column(Order = 3)]
        public Guid ROrderDtlId { get; set; }
       
        /// <summary>
        /// 退換單ID
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid ROrderId { get; set; }
        /// <summary>
        /// 縮略圖片路徑
        /// </summary>
        [Required]
        [DefaultValue("")]
        [Column(Order = 5, TypeName = "varchar")]
        [StringLength(200)]
        public string ImageS { get; set; }
        /// <summary>
        /// 原尺寸圖片路徑
        /// </summary>
        [Required]
        [DefaultValue("")]
        [Column(Order = 6, TypeName = "varchar")]
        [StringLength(200)]
        public string ImageB { get; set; }
        
        /// <summary>
        /// 圖片類別
        /// </summary>
        [Required]
        [Column(Order = 7)]
        public ReturnOrderImgType Type { get; set; }
        /// <summary>
        /// 付運日期
        /// </summary>
        [Column(Order = 8)]
        public DateTime? DeliveryDate { get; set; }
    }
}
