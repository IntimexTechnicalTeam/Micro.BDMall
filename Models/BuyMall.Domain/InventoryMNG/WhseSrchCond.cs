namespace BDMall.Domain
{
    /// <summary>
    /// 倉庫信息搜尋條件
    /// </summary>
    /// <remarks>前台交互對象</remarks>
    public class WhseSrchCond
    {
        /// <summary>
        /// 倉庫名稱
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 所屬商家Id
        /// </summary>
        public string MerchantId { get; set; }
    }
}
