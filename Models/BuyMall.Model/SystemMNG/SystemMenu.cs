namespace BDMall.Model
{
    public class SystemMenu : BaseEntity<int>
    {
        [Required]
        [Column(Order = 3)]
        public string Code { get; set; }
        [Required]
        [Column(Order = 4)]
        public Guid NameTransId { get; set; }
        [Column(Order = 5)]
        public string PageUrl { get; set; }
        [Column(Order = 6)]
        public string ImgUrl { get; set; }
        [Column(Order = 7)]
        public int ParentId { get; set; }
        [Column(Order = 8)]
        public Guid PermissionId { get; set; }
        [Column(Order = 9)]
        public Guid FunctionId { get; set; }
        [Column(Order = 10)]
        public Guid ModuleId { get; set; }
        [Column(Order = 11)]
        public int Seq { get; set; }
        /// <summary>
        /// 是否可以在Mobile Web端顯示
        /// </summary>
        [Required]
        [Column(Order = 12)]
        public bool IsMobileEnable { get; set; }
        /// <summary>
        /// 是否首頁顯示的菜單項
        /// </summary>
        [Required]
        [Column(Order = 13)]
        public bool IsHomeItem { get; set; }
    }
}
