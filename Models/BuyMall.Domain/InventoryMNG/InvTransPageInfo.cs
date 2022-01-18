using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    /// <summary>
    /// 庫存進貨/調度管理頁面信息
    /// </summary>
    public class InvTransPageInfo
    {
        /// <summary>
        /// 搜尋條件
        /// </summary>
        public InvTransSrchCond Condition { get; set; }
    }
}
