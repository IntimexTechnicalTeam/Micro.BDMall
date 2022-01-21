using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    /// <summary>
    /// 登录参数实体
    /// </summary>
    public class LoginInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage ="账号不能为空")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        public string Email { get; set; } = "";

        public string VerifyCode { get; set; }
    }
}
