namespace BDMall.Domain
{
    /// <summary>
    /// 庫存進貨/調度管理頁面搜尋條件
    /// </summary>
    public class InvTransSrchCond
    {
        /// <summary>
        /// 批號
        /// </summary>
        public string BatchNum { get; set; }

        /// <summary>
        /// 產品編號
        /// </summary>
        //public string ProdCode { get; set; }
        /// <summary>
        /// 產品編號列表
        /// </summary>
        public List<string> ProdCodeList { get; set; }
        /// <summary>
        /// 庫存屬性一
        /// </summary>
        public Guid Attr1 { get; set; }
        /// <summary>
        /// 庫存屬性二
        /// </summary>
        public Guid Attr2 { get; set; }
        /// <summary>
        /// 庫存屬性三
        /// </summary>
        public Guid Attr3 { get; set; }
        /// <summary>
        /// 商家Id
        /// </summary>
        public Guid MerchantId { get; set; }
        /// <summary>
        /// 倉庫ID
        /// </summary>
        public Guid WhId { get; set; }
    }
}
