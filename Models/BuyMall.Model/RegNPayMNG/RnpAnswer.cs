using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class RnpAnswer : BaseEntity<Guid>
    {

        public Guid QuestionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Display { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        public int? Max { get; set; }

        public int? Min { get; set; }

    }
}
