namespace BDMall.Model
{
    public class MemberGroup : BaseEntity<Guid>
    {
        /// <summary>
        /// 多语言ID
        /// </summary> 
        public Guid NameTransId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Remark { get; set; }


        /// <summary>
        /// 享受折扣,占百分比
        /// </summary>
        public decimal? Discount { get; set; }


        public Guid ParentId { get; set; }
    }
}
