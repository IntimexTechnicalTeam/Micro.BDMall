namespace BDMall.Model
{

    public class Permission : BaseEntity<Guid>
    {
        public Permission()
        {

        }



        /// <summary>
        /// 模塊
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string Module { get; set; }

        /// <summary>
        /// 功能
        /// </summary>

        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string Function { get; set; }

        /// <summary>
        /// 描述
        /// </summary>       
        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// 描述
        /// </summary>       
        [MaxLength(1000)]
        public string DescriptionTC { get; set; }

        [MaxLength(1000)]
        public string DescriptionSC { get; set; }

        public int Seq { get; set; }
        //public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
