using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    [Table("RolePermissions")]
    public class RolePermission : BaseEntity<Guid>
    {

       
        public Guid PermissionId { get; set; }

        public Guid RoleId { get; set; }


        //[ForeignKey("RoleId")]
        //public virtual Role Role { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}
