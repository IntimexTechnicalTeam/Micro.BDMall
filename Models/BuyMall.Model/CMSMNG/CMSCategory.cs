namespace BDMall.Model
{
    public class CMSCategory : BaseEntity<Guid>
    {

        public Guid ParentId { get; set; }
        public int Key { get; set; }
        public int Seq { get; set; }
        public int Style { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Desc_E { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Desc_C { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Desc_S { get; set; }

        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Desc_J { get; set; }

        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Name_E { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Name_C { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Name_S { get; set; }
        [StringLength(50)]
        [Required(AllowEmptyStrings = true)]
        public string Name_J { get; set; }
        [MaxLength(10000)]
        [Required(AllowEmptyStrings = true)]
        public string Content_E { get; set; }
        [MaxLength(10000)]
        [Required(AllowEmptyStrings = true)]
        public string Content_C { get; set; }
        [MaxLength(10000)]
        [Required(AllowEmptyStrings = true)]
        public string Content_S { get; set; }
        [MaxLength(10000)]
        [Required(AllowEmptyStrings = true)]
        public string Content_J { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = true)]
        public string Image { get; set; }
    


    }
}