using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class HotSalesDetailView
    {
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Qty { get; set; }

        public int? Week { get; set; } = 0;
    }
}
