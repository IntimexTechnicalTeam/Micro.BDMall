namespace BDMall.Domain
{
    /// <summary>
    /// 倉庫信息頁面信息
    /// </summary>
    /// <remarks>前端交互對象</remarks>
    public class WhsePageInfo
    {
        public string SortName { get; set; }
        public string SortOrder { get; set; }

        /// <summary>
        /// 倉庫信息搜尋條件
        /// </summary>
        public WhseSrchCond Condition { get; set; }
    }
}
