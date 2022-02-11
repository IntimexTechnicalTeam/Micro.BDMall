using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDMall.Enums;
using Web.Framework;

namespace BDMall.Domain
{
    public class MicroOrderCond
    {
        /// <summary>
        /// 訂單狀態
        /// </summary>
        public OrderStatus StatusCode { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}
