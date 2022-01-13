﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    /// <summary>
    /// 庫存流動報告頁面信息
    /// </summary>
    public class InvFlowPageInfo
    {
        /// <summary>
        /// 庫存流動數據搜尋條件
        /// </summary>
        public InvFlowSrchCond Condition { get; set; } = new InvFlowSrchCond();
    }
}
