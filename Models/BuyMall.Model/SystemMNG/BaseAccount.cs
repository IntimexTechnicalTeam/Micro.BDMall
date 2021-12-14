using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class BaseAccount<TKey> : BaseEntity<TKey>
    {
        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 10)]
        //[Display(Name = "Account", ResourceType = typeof(Resources.Model))]
        public string Account { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 11)]
        //[Display(Name = "Email", ResourceType = typeof(Resources.Model))]
        public string Email { get; set; }


        [Required]
        [MaxLength(200)]
        //[Column(TypeName = "varchar")]
        public string Password { get; set; }
    }
}

