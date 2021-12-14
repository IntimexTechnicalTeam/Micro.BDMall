using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    public enum LoginType
    {
        /// <summary>
        /// admin
        /// </summary>
        Admin = 1,

        /// <summary>
        /// 商家
        /// </summary>
        Merchant = 2,

        /// <summary>
        /// 第三方商家
        /// </summary>
        ThirdMerchantLink = 3,

        /// <summary>
        /// 会员
        /// </summary>
        Member = 4,

        /// <summary>
        /// 匿名用户
        /// </summary>
        TempUser = 5,

        /// <summary>
        /// 第三方会员
        /// </summary>
        ThirdMemberLink = 6,

    }
}
