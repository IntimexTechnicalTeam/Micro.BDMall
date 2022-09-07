namespace BDMall.Domain
{
    /// <summary>
    /// 庫存交易詳細項資料
    /// </summary>
    /// <remarks>前端交互用</remarks>
    public class InvTransItemView
    {
        /// <summary>
        /// 發貨方ID
        /// </summary>
        public Guid TransFrom { get; set; }
        /// <summary>
        /// 發貨方名稱
        /// </summary>
        public string TransFromDesc { get; set; }
        /// <summary>
        /// 收貨方ID
        /// </summary>
        public Guid TransTo { get; set; }
        /// <summary>
        /// 收貨方名稱
        /// </summary>
        public string TransToDesc { get; set; }
        /// <summary>
        /// 庫存單元編號
        /// </summary>
        public Guid Sku { get; set; }
        /// <summary>
        /// 目錄ID
        /// </summary>
        public Guid CatalogId { get; set; }
        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProdCode { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProdName { get; set; }
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
        /// 庫存屬性一描述
        /// </summary>
        public string Attr1Desc { get; set; }
        /// <summary>
        /// 庫存屬性二描述
        /// </summary>
        public string Attr2Desc { get; set; }
        /// <summary>
        /// 庫存屬性三描述
        /// </summary>
        public string Attr3Desc { get; set; }
        /// <summary>
        /// 交易數量
        /// </summary>
        public int TransQty { get; set; }
        /// <summary>
        /// 退回數量
        /// </summary>
        public int ReturnQty { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 是否有選中
        /// </summary>
        public bool IsChecked { get; set; }
        /// <summary>
        /// 可銷售數量
        /// </summary>
        public decimal SalesQty { get; set; }
    }
}
