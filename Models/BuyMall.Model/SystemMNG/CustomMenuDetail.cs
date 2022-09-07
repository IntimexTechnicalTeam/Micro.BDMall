namespace BDMall.Model
{
    /// <summary>
    /// 翻译
    /// </summary>
    public class CustomMenuDetail : BaseEntity<Guid>
    {
        [Required]
        [Column(Order = 3)]
        public Guid MenuId { get; set; }

        [Required]
        [Column(Order = 4, TypeName = "varchar")]
        [StringLength(100)]
        public string Value { get; set; }

        [Required]
        [Column(Order = 5)]
        public int Seq { get; set; }

        [Required]
        [Column(Order = 6)]
        public bool IsAnchor { get; set; }
    }
}
