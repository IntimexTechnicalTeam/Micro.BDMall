namespace BDMall.Model
{
    public class SystemEmail : BaseEntity<Guid>
    {
        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string Type { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string SendFrom { get; set; }

        [MaxLength(1000)]
        [Column(TypeName = "varchar")]
        public string SendTo { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "varchar")]
        public string CC { get; set; }

        [MaxLength(500)]
        public string Subject { get; set; }

        public Guid ContentId { get; set; }

        public DateTime? ExpectSendDate { get; set; }

        public DateTime? SendDate { get; set; }

        public int FailCount { get; set; }

        public bool IsSucceeded { get; set; }

        public EmailerStatus Status { get; set; }
    }
}
