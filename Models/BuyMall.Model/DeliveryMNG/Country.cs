namespace BDMall.Model
{
    public class Country : BaseEntity<int>
    {
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Name_e { get; set; }
        [StringLength(50)]
        public string Name_c { get; set; }
        [StringLength(50)]
        public string Name_s { get; set; }
        [StringLength(50)]
        public string Name_j { get; set; }

        /// <summary>
        /// 三位字母 ec-ship 
        /// </summary>
        [StringLength(5)]
        public string Code { get; set; }

        /// <summary>
        /// 兩位字母
        /// </summary>
        [StringLength(2)]
        public string Code2 { get; set; }

        /// <summary>
        /// 三位字母
        /// </summary>
        [StringLength(3)]
        public string Code3 { get; set; }

        public int Seq { get; set; }
        /// <summary>
        /// 郵政編碼是否必填
        /// </summary>
        [Required]
        [DefaultValue(false)]
        public bool IsNeedPostalCode { get; set; }
    }
}