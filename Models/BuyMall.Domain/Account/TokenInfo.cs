using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    /// <summary>
    /// token参数实体
    /// </summary>
    public class TokenInfo
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        public string Account { get; set; }

        public string Email { get; set; } = "";

        public Language Lang { get; set; } = Language.C;

        public LoginType LoginType { get; set; } = LoginType.TempUser;

        public string CurrencyCode { get; set; } = "HKD";

        public bool IsLogin { get; set; } = false;

       
    }
}
