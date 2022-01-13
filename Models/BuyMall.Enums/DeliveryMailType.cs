using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    public enum DeliveryMailType
    {
        /// <summary>
        /// 派送
        /// </summary>
        D1 = 1,
        /// <summary>
        /// 柜台领取
        /// </summary>
        CC = 2,
        /// <summary>
        /// 智邮站
        /// </summary>
        PL = 3,
        /// <summary>
        /// 邮包
        /// </summary>
        E = 4,
        /// <summary>
        /// 智郵站非MCN獲取方式
        /// </summary>
        SP = 5

    }
}
