namespace BDMall.Model
{
    /// <summary>
    /// 支付方式
    /// </summary>
    public class PaymentMethod : BaseEntity<Guid>
    {
        /// <summary>
        /// 名称多语言ID
        /// </summary>
        [Column(Order = 3)]
        public Guid NameTransId { get; set; }
        /// <summary>
        /// 备注ID
        /// </summary>
        [Column(Order = 4)]
        public Guid RemarkTransId { get; set; }
        /// <summary>
        /// 圖片
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [StringLength(200)]
        [Column(Order = 5)]
        public string Image { get; set; }
        [StringLength(100)]
        [Column(Order = 6)]
        public string BankAccount { get; set; }
        /// <summary>
        /// code
        /// </summary>
        [Column(Order = 7)]
        [StringLength(10)]
        public string Code { get; set; }
        /// <summary>
        /// 支付服務手續費率
        /// </summary>
        [Column(Order = 8)]
        public decimal ServRate { get; set; }

    }
}
