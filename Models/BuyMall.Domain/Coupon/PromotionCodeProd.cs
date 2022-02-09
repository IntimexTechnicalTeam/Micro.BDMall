using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    /// <summary>
    /// 推廣優惠碼的指定優惠商品資料
    /// </summary>
    public class PromotionCodeProd
    {
        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProdCode { get; set; }
        ///// <summary>
        ///// 產品名稱
        ///// </summary>
        //public string ProdName { get; set; }
        /// <summary>
        /// 是否存在折扣優惠
        /// </summary>
        public bool IsDiscount { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 小計
        /// </summary>
        public decimal Amount
        {
            get
            {
                return UnitPrice * Qty;
            }
        }
    }
}
