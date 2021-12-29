using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class SystemEmailsCond : PageInfo
    {
        public string Email { get; set; }
        public bool IsSucceed { get; set; }
    }
}
