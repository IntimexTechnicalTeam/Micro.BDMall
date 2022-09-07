namespace BDMall.Domain
{
    public class MenuDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Title { get; set; }

        public int Type { get; set; }

        public string Url { get; set; }

        public string Image { get; set; }

        public ValueModel Value { get; set; }

        public int Level { get; set; }

        public string ParentId { get; set; }

        /// <summary>
        /// 是否置頂
        /// </summary>
        public bool PlaceTop { get; set; }
        public List<MenuDetailModel> Childs { get; set; }

        public int RedirectType { get; set; }

        public string RedirectValue { get; set; }

        public bool IsNewWin { get; set; }

        public int Seq { get; set; }

        public Guid NameTransId { get; set; }
        public Guid TitleTransId { get; set; }
        public Guid ImageTransId { get; set; }
    }
}
