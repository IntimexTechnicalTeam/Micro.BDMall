namespace BDMall.Model
{
    public class RnpForm : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string DescTop { get; set; }
        [MaxLength(500)]
        public string DescBottom { get; set; }

        public bool IsPayment { get; set; }

        public int? CurrencyId { get; set; }

        public bool IsLimit { get; set; }

        public int? Limit { get; set; }

        public bool IsNotice { get; set; }

        [StringLength(50)]
        public string NoticeEmail { get; set; }

        public bool IsSign { get; set; }

        public new bool IsDeleted { get; set; }

        public Guid? CopyFormId { get; set; }

        public Guid? RootFormId { get; set; }
    }
}
