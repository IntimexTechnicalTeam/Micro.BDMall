namespace BDMall.Model
{
    public class CodeMaster : BaseEntity<int>
    {
        [StringLength(20)]
        [Column(Order = 3)]
        public string Module { get; set; }

        [StringLength(20)]
        [Column(Order = 4)]
        public string Function { get; set; }

        [StringLength(20)]
        [Column(Order = 5)]
        public string Key { get; set; }

        [Required]
        [StringLength(200)]
        [Column(Order = 6)]
        public string Value { get; set; }

        [Column(Order = 7)]
        public Guid DescTransId { get; set; }

        [Column(Order = 8)]
        public string Remark { get; set; }

    }

}
