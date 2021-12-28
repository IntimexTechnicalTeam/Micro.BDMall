using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class CurrentUser
    {
        public string Token { get; set; } = "";

        public string UserId { get; set; } = Guid.NewGuid().ToString(); 

        public Language Lang { get; set; } = Language.C;

        public LoginType LoginType { get; set; } = LoginType.TempUser;

        public string CurrencyCode { get; set; } = "HKD";

        public bool IsLogin { get; set; } = false;

        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();

        public Guid MechantId { get; set; }

        public bool IsMerchant => LoginType <= LoginType.ThirdMerchantLink ? true : false;
    }


    public class CurrentUser<T> : CurrentUser
    { 
        public T UserData { get; set; }
    }
}
