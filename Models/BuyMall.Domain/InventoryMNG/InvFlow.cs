namespace BDMall.Domain
{
    /// <summary>
    /// 庫存流動數據
    /// </summary>
    public class InvFlow
    {
        /// <summary>
        /// 庫存單元編碼
        /// </summary>
        public Guid Sku { get; set; }
        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 產品記錄ID
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// 庫存屬性一
        /// </summary>
        public Guid Attr1 { get; set; }
        /// <summary>
        /// 庫存屬性一描述
        /// </summary>
        public string Attr1Desc { get; set; }
        /// <summary>
        /// 庫存屬性二
        /// </summary>
        public Guid Attr2 { get; set; }
        /// <summary>
        /// 庫存屬性二描述
        /// </summary>
        public string Attr2Desc { get; set; }
        /// <summary>
        /// 庫存屬性三
        /// </summary>
        public Guid Attr3 { get; set; }
        /// <summary>
        /// 庫存屬性三描述
        /// </summary>
        public string Attr3Desc { get; set; }
        /// <summary>
        /// 庫存交易類型
        /// </summary>
        public InvTransType TransType { get; set; }
        /// <summary>
        /// 庫存交易類型名稱
        /// </summary>
        public string TransTypeDesc { get; set; }
        /// <summary>
        /// 交易時間
        /// </summary>
        public DateTime TransDate { get; set; }
        /// <summary>
        /// 交易數量
        /// </summary>
        public decimal TransQty { get; set; }
        /// <summary>
        /// 參考單號
        /// </summary>
        public string RefNo { get; set; }
        /// <summary>
        /// 批號
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remarks { get; set; }

        public string Handler { get; set; }

        public string ImagePath { get; set; }

        /// <summary>
        /// 倉庫名稱
        /// </summary>
        public string WhName { get; set; }

        /// <summary>
        /// 出入倉類型
        /// </summary>
        public string IOTypeDesc { get; set; }

        public DateTime CreateDate { get; set; }

        public InvFlow()
        {
            ProductCode = string.Empty;
            Attr1Desc = string.Empty;
            Attr2Desc = string.Empty;
            Attr3Desc = string.Empty;
            TransTypeDesc = string.Empty;
            RefNo = string.Empty;
            Remarks = string.Empty;
            WhName = string.Empty;
            IOTypeDesc = string.Empty;
        }
    }
}
