namespace BDMall.Model
{
    /// <summary>
    /// 倉庫資料
    /// </summary>
    public class Warehouse : BaseEntity<Guid>
    {
      
        [Required]
        [Column(Order = 3)]
        public Guid NameTransId { get; set; }

        [Column(Order = 4)]
        public Guid AddressTransId { get; set; }
        
        [Column(Order = 5)]
        public Guid ContactTransId { get; set; }

        /// <summary>
        /// 聯繫電話
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(30)]
        [DefaultValue("")]
        [Column(TypeName = "varchar", Order = 6)]
        public string PhoneNum { get; set; } = "";
        /// <summary>
        /// 郵政編號
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(50)]
        [DefaultValue("")]
        [Column(TypeName = "nvarchar", Order = 7)]
        public string PostalCode { get; set; } = "";
        /// <summary>
        /// 備註
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(200)]
        [DefaultValue("")]
        [Column(TypeName = "nvarchar", Order = 8)]
        public string Remarks { get; set; } = "";
        /// <summary>
        /// 所屬商家記錄ID
        /// </summary>
        [Required]
        [Column(Order = 9)]
        public Guid MerchantId { get; set; }
        /// <summary>
        /// 所屬商家信息
        /// </summary>
        [ForeignKey("MerchantId")]
        public virtual Merchant MerchantInfo { get; set; }
        /// <summary>
        /// 成本中心
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(45)]
        [Column(TypeName = "varchar", Order = 10)]
        public string CostCenter { get; set; } = "";
        /// <summary>
        /// 銀行賬號
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(45)]
        [Column(TypeName = "varchar", Order = 11)]
        public string AccountCode { get; set; } = "";

    }
}
