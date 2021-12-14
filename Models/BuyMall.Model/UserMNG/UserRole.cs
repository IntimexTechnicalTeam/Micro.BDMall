using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
   
    public class UserRole : BaseEntity<Guid>
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public Guid UserId { get; set; }


        [ForeignKey("RoleId")]
        public Role Role { get; set; }


        //[ForeignKey("UserId")]
        //public User User { get; set; }
    }
}
