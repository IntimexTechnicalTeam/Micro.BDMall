using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class InvFlowView
    {
        /// <summary>
        /// 庫存單元編碼
        /// </summary>
        public Guid SKU { get; set; }
        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 產品ID
        /// </summary>
        public Guid ProductId { get; set; }
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
        /// 庫存流動類型名稱
        /// </summary>
        public string ActionType { get; set; }
        /// <summary>
        /// 流動時間
        /// </summary>
        public string TransDate { get; set; }
        /// <summary>
        /// 流動數量
        /// </summary>
        public decimal TransQty { get; set; }
        /// <summary>
        /// 供應商名稱/客戶
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 參考單號
        /// </summary>
        public string RefNo { get; set; }
        /// <summary>
        /// 經手人
        /// </summary>
        public string Handler { get; set; }
        /// <summary>
        /// 批號
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImgPath { get; set; }
        /// <summary>
        /// 倉庫名稱
        /// </summary>
        public string WhName { get; set; }

        /// <summary>
        /// 出入倉類型描述
        /// </summary>
        public string IOTypeDesc { get; set; }
    }
}
