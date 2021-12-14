using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    public enum MQType
    {
        None = 0,
        UpdateInvt = 1,
        HasChangeTransinFun = 2,
        TranPointsData = 3,                          //同步数据
        UpdateProductLimitPrice = 4,
    }

    public enum MQState
    {
        /// <summary>
        /// 未处理
        /// </summary>
        UnDeal = 1,
        /// <summary>
        /// 已处理
        /// </summary>
        Deal = 2,
        /// <summary>
        /// 异常
        /// </summary>
        Exception = 3
    }
}
