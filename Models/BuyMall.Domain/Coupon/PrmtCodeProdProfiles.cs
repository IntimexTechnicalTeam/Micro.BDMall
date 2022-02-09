using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class PrmtCodeProdProfiles
    {
        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProdCode { get; set; }
        /// <summary>
        /// 總數量
        /// </summary>
        public int TotalQty { get; set; }
        /// <summary>
        /// 總價
        /// </summary>
        public decimal TotalAmt { get; set; }
        /// <summary>
        /// 原總價
        /// </summary>
        public decimal OriginalTotalAmt { get; set; }
        /// <summary>
        /// 已優惠數量
        /// </summary>
        public int DiscountQty { get; set; }
        /// <summary>
        /// 優惠合計
        /// </summary>
        public decimal DiscountAmt { get; set; }
        /// <summary>
        /// 已優惠額
        /// </summary>
        public decimal DiscountVal { get; set; }

        public PromotionCodeProd OriginalProd { get; set; }

        public PromotionCodeProd DiscountProd { get; set; }
    }
}
