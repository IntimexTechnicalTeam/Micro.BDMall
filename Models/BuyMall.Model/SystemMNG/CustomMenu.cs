namespace BDMall.Model
{
    /// <summary>
    /// 翻译
    /// </summary>
    public class CustomMenu : BaseEntity<Guid>
    {
        [Required]
        [Column(Order = 3)]
        public Guid ParentId { get; set; }

        [Required]
        [Column(Order = 4, TypeName = "nvarchar")]
        [StringLength(400)]
        public string Key { get; set; }

        [Required]
        [Column(Order = 5)]
        public Guid NameTransId { get; set; }

        [Required]
        [Column(Order = 6)]
        public Guid TitleTransId { get; set; }

        [Required]
        [Column(Order = 7)]
        public Guid ImageTransId { get; set; }

        [Required]
        [Column(Order = 8)]
        public int Seq { get; set; }

        [Required]
        [Column(Order = 9)]
        public int PositionType { get; set; }

        [Required]
        [Column(Order = 10)]
        public int LinkType { get; set; }

        [Required]
        [Column(Order = 11)]
        public bool ShowNext { get; set; }

        [Required]
        [Column(Order = 12)]
        public bool IsShow { get; set; }

        [Required]
        [Column(Order = 13)]
        public bool PlaceTop { get; set; }

        [Required]
        [Column(Order = 14)]
        public int RedirectLinkType { get; set; }

        [Required]
        [Column(Order = 15, TypeName = "varchar")]
        [StringLength(200)]
        public string RedirectLinkValue { get; set; }

        [Column(Order = 16)]
        public bool IsNewWin { get; set; }
    }
}
