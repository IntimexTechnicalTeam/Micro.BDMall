namespace BDMall.Domain
{
    /// <summary>
    /// 庫存流動數據搜尋條件
    /// </summary>
    public class InvFlowSrchCond:PageInfo
    {
        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 交易查詢起始時間
        /// </summary>
        public string TransBeginDate { get; set; }
        /// <summary>
        /// 交易查詢結束時間
        /// </summary>
        public string TransEndDate { get; set; }
        /// <summary>
        /// 庫存交易類型列表(逗號分隔)
        /// </summary>
        public string TransTypeList { get; set; }
        /// <summary>
        /// 種類目錄記錄ID
        /// </summary>
        public Guid CategoryId { get; set; }
        /// <summary>
        /// 庫存屬性I
        /// </summary>
        public Guid Attr1 { get; set; }
        /// <summary>
        /// 庫存屬性II
        /// </summary>
        public Guid Attr2 { get; set; }
        /// <summary>
        /// 庫存屬性III
        /// </summary>
        public Guid Attr3 { get; set; }
       
        /// <summary>
        /// 排序方式
        /// </summary>
        public string SortBy { get; set; }
        /// <summary>
        /// 商家Id
        /// </summary>
        public Guid MerchantId { get; set; }
    }
}
