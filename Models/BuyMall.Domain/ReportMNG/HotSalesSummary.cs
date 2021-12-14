using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class HotSalesSummary
    {
        /// <summary>
        /// 序號
        /// </summary>
        public int Seq { get; set; }
        /// <summary>
        /// SKU
        /// </summary>
        public Guid Sku { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 匯總年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 匯總月份
        /// </summary>
        public int? Month { get; set; }
        /// <summary>
        /// 匯總週數
        /// </summary>
        public int? Week { get; set; }
        /// <summary>
        /// 匯總日期
        /// </summary>
        public int? Day { get; set; }
        /// <summary>
        /// 匯總數量
        /// </summary>
        public int TotalSalesQty { get; set; }

        public int Qty => this.TotalSalesQty;
    }
}
