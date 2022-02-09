using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    /// <summary>
    /// 智邮站类型
    /// </summary>
    public enum IPSType
    {
        Default=0,

        /// <summary>
        /// 用MCN码获取站点
        /// </summary>
        MCN = 1,
        /// <summary>
        /// 手动选择站点并填写手机号码
        /// </summary>
        Phone = 2
    }
}
