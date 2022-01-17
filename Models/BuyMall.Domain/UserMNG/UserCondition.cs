using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class UserCondition
    {

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public PageInfo PageInfo { get; set; } = new PageInfo();
    }
}

