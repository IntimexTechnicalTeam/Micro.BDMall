namespace BDMall.Domain
{
    public class InvSummaryPageInfo
    {
        /// <summary>
        /// 目錄ID
        /// </summary>
        public int CatelogID { get; set; }
        /// <summary>
        /// 庫存匯總信息搜尋條件
        /// </summary>
        public InvSrchCond Condition { get; set; } = new InvSrchCond();
    }
}
