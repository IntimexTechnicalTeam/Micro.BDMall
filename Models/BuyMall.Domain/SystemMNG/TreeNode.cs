namespace BDMall.Domain
{
    public class TreeNode
    {

        public int Id { get; set; }

        public int ParentId { get; set; }
        public string Path { get; set; }
        public string PathName { get; set; }
        public string Code { get; set; }

        public string Text { get; set; }

        public string Url { get; set; }

        public int Seq { get; set; }

        public List<TreeNode> Children { get; set; }

        public bool Collapse { get; set; }

        public int Level { get; set; }

        public string Img { get; set; }

        public string ImgPath { get; set; }

        //public List<MutiLanguage> MutiLanguage { get; set; }

        public List<string> Attrs { get; set; }

        public bool IsChange { get; set; }

        /// <summary>
        /// 是否可以在Mobile Web端顯示
        /// </summary>
        public bool IsMobileEnable { get; set; }

        /// <summary>
        /// 是否首頁顯示的菜單項
        /// </summary>
        public bool IsHomeItem { get; set; }

        public bool IsActive { get; set; }
    }
}
