namespace BDMall.Model
{
    /// <summary>
    /// 供應商資料
    /// </summary>
    public class Supplier : BaseEntity<Guid>
    {     
        [Required]
        [Column(Order = 3)]
        public Guid NameTransId { get; set; }
        
        /// <summary>
        /// 聯繫人
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(50)]
        [DefaultValue("")]
        [Column(TypeName = "nvarchar", Order = 4)]
        public string Contact { get; set; }
        /// <summary>
        /// 聯繫電話
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(30)]
        [DefaultValue("")]
        [Column(TypeName = "varchar", Order = 5)]
        public string PhoneNum { get; set; }
        /// <summary>
        /// 傳真電話
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(30)]
        [DefaultValue("")]
        [Column(TypeName = "varchar", Order = 6)]
        public string FaxNum { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(200)]
        [DefaultValue("")]
        [Column(TypeName = "nvarchar", Order = 7)]
        public string Remarks { get; set; }
        /// <summary>
        /// 所屬商家記錄ID
        /// </summary>
        [Required]
        [Column(Order = 8)]
        public Guid MerchantId { get; set; }
       
        public Supplier()
        {
            Contact = string.Empty;
            PhoneNum = string.Empty;
            FaxNum = string.Empty;
            Remarks = string.Empty;
            
        }
    }
}
