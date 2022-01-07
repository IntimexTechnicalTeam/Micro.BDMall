using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class PermissionCondition : PageInfo
    {
        public PermissionCondition()
        {
            this.IsActive = false;
            this.IsDeleted = false;
        }
        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }


        public string Module { get; set; }
        public string Function { get; set; }



    }
}
