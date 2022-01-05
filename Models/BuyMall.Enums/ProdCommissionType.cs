using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    /// <summary>
    /// 產品佣金結算類別
    /// </summary>
    public enum ProdCommissionType
    {
        /// <summary>
        /// 繼承商家設定
        /// </summary>
        MerchInheriting = 1,
        /// <summary>
        /// 固定金額
        /// </summary>
        FixedValue = 2,
        /// <summary>
        /// 固定比率
        /// </summary>
        FixedRate = 3
    }
}
