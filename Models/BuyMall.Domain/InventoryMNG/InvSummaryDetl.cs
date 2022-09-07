namespace BDMall.Domain
{
    /// <summary>
    /// 庫存匯總詳細信息
    /// </summary>
    public class InvSummaryDetl
    {
        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImgPath { get; set; }
        /// <summary>
        /// 庫存單元編碼
        /// </summary>
        public Guid Sku { get; set; }
        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 庫存屬性一描述
        /// </summary>
        public string AttrValue1Desc { get; set; }
        /// <summary>
        /// 庫存屬性二描述
        /// </summary>
        public string AttrValue2Desc { get; set; }
        /// <summary>
        /// 庫存屬性三描述
        /// </summary>
        public string AttrValue3Desc { get; set; }
        /// <summary>
        /// 倉庫名稱
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 庫存數量
        /// </summary>
        public int InventoryQty { get; set; }

        public InvSummaryDetl()
        {
            this.ProductCode = string.Empty;
            this.ProductName = string.Empty;
            this.WarehouseName = string.Empty;
            this.AttrValue1Desc = string.Empty;
            this.AttrValue2Desc = string.Empty;
            this.AttrValue3Desc = string.Empty;
        }
    }
}
