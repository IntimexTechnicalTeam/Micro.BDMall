using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    /// <summary>
    /// 庫存信息搜尋條件
    /// </summary>
    public class InvSrchCond : PageInfo
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
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 種類目錄記錄ID
        /// </summary>
        public Guid CategoryId { get; set; }
        /// <summary>
        /// 庫存屬性I
        /// </summary>
        public Guid AttributeI { get; set; }
        /// <summary>
        /// 庫存屬性II
        /// </summary>
        public Guid AttributeII { get; set; }
        /// <summary>
        /// 庫存屬性III
        /// </summary>
        public Guid AttributeIII { get; set; }
        /// <summary>
        /// 可銷售數量上限
        /// </summary>
        public int? SalesQtyUpperLimit { get; set; }
        /// <summary>
        /// 可銷售數量下限
        /// </summary>
        public int? SalesQtyLowerLimit { get; set; }
        /// <summary>
        /// 可銷售數排序方式
        /// </summary>
        public string SalesQtySortType { get; set; }
        /// <summary>
        /// 商家Id
        /// </summary>
        public Guid MerchantId { get; set; }
    }
}
