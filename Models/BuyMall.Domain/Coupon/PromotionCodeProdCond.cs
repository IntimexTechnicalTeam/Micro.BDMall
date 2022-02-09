using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class PromotionCodeProdCond
    {
        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProdCode { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Qty { get; set; }
    }
}
