using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    /// <summary>
    /// 退貨方式
    /// </summary>
    public enum ReturnOrderType
    {
        /// <summary>
        /// 現金
        /// </summary>
        Cash = 1,
        /// <summary>
        /// 積分
        /// </summary>
        MallFun = 2,
        /// <summary>
        /// 退換貨品
        /// </summary>
        ChangeGoods = 3
    }
}
