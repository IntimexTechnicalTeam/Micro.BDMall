using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class SalesOrderDetailView
    {
        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Qty { get; set; }
    }
}
