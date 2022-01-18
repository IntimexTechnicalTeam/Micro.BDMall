using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.ECShip.Model.Posting
{
    public class ECShipOrderItemInfo
    {
        /// <summary>
        /// 产品描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 产品数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 产品重量
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 货币Code
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 货物的总价钱
        /// </summary>
        public decimal TotalPrice { get; set; }

    }
}
