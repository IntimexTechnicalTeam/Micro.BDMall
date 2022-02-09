using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    public enum CouponStatus
    {
        ALL = 0,
        Used = 1,//已經使用
        NoUsed = 2,//未使用
        Active = 3,//有效的
        DisActive = 4//失效的
    }
}
