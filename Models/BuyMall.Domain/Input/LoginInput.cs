using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class LoginInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get;set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; } = "";

        /// <summary>
        /// 登录类型
        /// </summary>
        public LoginType LoginType { get; set; } = LoginType.Member;
    }
}
