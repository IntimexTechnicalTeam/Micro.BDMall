namespace BDMall.Domain
{
    public class MenuItem //: BaseProperty
    {
        public int Id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称（根據界面語言顯示）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名称多语言ID
        /// </summary>
        public Guid NameTransId { get; set; }

        public List<MutiLanguage> NameTranslation { get; set; }

        /// <summary>
        /// 访问路径
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// 图片名
        /// </summary>
        public string Img { get; set; }

        /// <summary>
        /// 菜单图片
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 上级菜单ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        public Guid FunctionId { get; set; }

        /// <summary>
        /// 模块Id
        /// </summary>
        public Guid ModuleId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 是否可以在Mobile Web端顯示
        /// </summary>
        public bool IsMobileEnable { get; set; }

        /// <summary>
        /// 是否首頁顯示的菜單項
        /// </summary>
        public bool IsHomeItem { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
