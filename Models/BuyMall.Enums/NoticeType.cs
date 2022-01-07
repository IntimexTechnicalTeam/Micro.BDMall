using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    /// <summary>
    /// 系統消息通知類別
    /// </summary>
    public enum NoticeType
    {
        /// <summary>
        /// 電郵
        /// </summary>
        Email = 1,
        /// <summary>
        /// 站內信
        /// </summary>
        InteractMessage = 2,
        WhatsApp = 3
    }
}
