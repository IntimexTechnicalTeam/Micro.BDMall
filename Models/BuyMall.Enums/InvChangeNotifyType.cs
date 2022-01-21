using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum InvChangeNotifyType
    {
        /// <summary>
        /// 低於安全庫存量
        /// </summary>
        LowThanSaftey = 0,
        /// <summary>
        /// 售罄
        /// </summary>
        SoldOut = 1
    }
}
