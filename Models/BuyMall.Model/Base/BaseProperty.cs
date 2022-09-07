namespace BDMall.Models
{
    public abstract class BaseProperty
    {
        [Column(Order = 98)]
        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Column(Order = 99)]
        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Required]
        [Column(Order = 100)]
        public DateTime CreateDate { get; set; }

        [Column(Order = 101)]
        public DateTime? UpdateDate { get; set; }

        [Required]
        //[MaxLength(36)]
        [Column(Order = 102)]
        public Guid CreateBy { get; set; }

        //[MaxLength(36)]
        [Column(Order = 103)]
        public Guid? UpdateBy { get; set; }



    }
}
