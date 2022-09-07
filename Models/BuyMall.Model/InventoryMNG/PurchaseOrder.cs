namespace BDMall.Model
{
    /// <summary>
    /// 採購單
    /// </summary>
    public class PurchaseOrder : BaseEntity<Guid>
    {
        /// <summary>
        /// 單號
        /// </summary>
        [Required]
        [MaxLength(30)]
        [DefaultValue("")]
        [Column(TypeName = "varchar", Order = 3)]
        public string OrderNo { get; set; } = "";
        /// <summary>
        /// 供應商ID
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid SupplierId { get; set; }
        //[ForeignKey("SupplierId")]
        //public virtual Supplier SupplierInfo { get; set; }
        /// <summary>
        /// 存放倉庫ID
        /// </summary>
        [Required]
        [Column(Order = 5)]
        public Guid WHId { get; set; }
        //[ForeignKey("WHId")]
        //public virtual Warehouse ImportWarehouse { get; set; }
        /// <summary>
        /// 批號
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(50)]
        [DefaultValue("")]
        [Column(TypeName = "nvarchar", Order = 6)]
        public string BatchNum { get; set; } = "";
        /// <summary>
        /// 入庫時間
        /// </summary>
        [Required]
        [Column(Order = 7)]
        public DateTime InDate { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(200)]
        [DefaultValue("")]
        [Column(TypeName = "nvarchar", Order = 8)]
        public string Remarks { get; set; } = "";
    }
}
