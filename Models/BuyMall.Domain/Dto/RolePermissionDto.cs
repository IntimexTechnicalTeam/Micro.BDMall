using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class RolePermissionDto:BaseDto
    {
        public Guid PermissionId { get; set; } 

        public Guid RoleId { get; set; }

    }
}
