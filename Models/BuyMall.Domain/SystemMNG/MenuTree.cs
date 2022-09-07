namespace BDMall.Domain
{
    public class MenuTree
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public int Seq { get; set; }

        /// <summary>
        /// 是否打開新窗口
        /// </summary>
        public bool IsNewWin { get; set; }
    }
}
