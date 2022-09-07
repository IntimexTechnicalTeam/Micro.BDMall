namespace BDMall.Model
{
    public class Merchant : BaseEntity<Guid>
    {
        /// <summary>
        /// 編號（Supplier ID）
        /// </summary>
        [Required]
        [MaxLength(25)]
        [Column(TypeName = "varchar", Order = 3)]
        public string MerchNo { get; set; }
        [Required]
        [Column(Order = 4)]
        public Guid NameTransId { get; set; }
       
        [Required]
        [Column(Order = 5)]
        public Guid ContactTransId { get; set; }
       
        /// <summary>
        /// 聯繫電話
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(30)]
        [Column(TypeName = "varchar", Order = 6)]
        public string ContactPhoneNum { get; set; }
        /// <summary>
        /// 傳真號碼
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(30)]
        [Column(TypeName = "varchar", Order = 7)]
        public string FaxNum { get; set; }
        [Required]
        [Column(Order = 8)]
        public Guid ContactAddrTransId { get; set; }
        [Required]
        [Column(Order = 9)]
        public Guid ContactAddr2TransId { get; set; }
        [Required]
        [Column(Order = 10)]
        public Guid ContactAddr3TransId { get; set; }
        [Required]
        [Column(Order = 11)]
        public Guid ContactAddr4TransId { get; set; }

        /// <summary>
        /// 聯繫電郵地址
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(50)]
        [Column(TypeName = "varchar", Order = 12)]
        public string ContactEmail { get; set; }
        /// <summary>
        /// 訂單電郵地址
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(300)]
        [Column(TypeName = "varchar", Order = 13)]
        public string OrderEmail { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [Required]
        [Column(Order = 14)]
        public Guid RemarksTransId { get; set; }

        /// <summary>
        /// 是否外部商家，默認是
        /// </summary>
        [Column(Order = 15)]
        public bool IsExternal { get; set; }

        [Column(Order = 16)]
        public MerchantType MerchantType { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "varchar", Order = 17)]
        public string GCP { get; set; }

        /// <summary>
        /// 使用語言
        /// </summary>
        [Column(Order = 18)]
        public Language Language { get; set; }

        /// <summary>
        /// 佣金率
        /// </summary>
        [Column(Order = 19)]
        public decimal CommissionRate { get; set; }

        /// <summary>
        /// 是否Transin商家
        /// </summary>
        [Column(Order = 20)]
        public bool IsTransin { get; set; }

        /// <summary>
        /// 是否香港商家
        /// </summary>
        [Column(Order = 21)]
        public bool IsHongKong { get; set; }

        /// <summary>
        /// 商家銀行賬號
        /// </summary>
        [MaxLength(200)]
        [Column(TypeName = "varchar", Order = 22)]
        public string BankAccount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 23)]
        public string AppId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 24)]
        public string AppSecret { get; set; }

    }
}
