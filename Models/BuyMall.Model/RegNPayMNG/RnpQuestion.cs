using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class RnpQuestion : BaseEntity<Guid>
    {

        public Guid FormId { get; set; }

        [StringLength(200)]
        public string Content { get; set; }

        [StringLength(100)]
        public string DataType { get; set; }

        public int? Type { get; set; }

        public bool? IsRequired { get; set; }

        public int? Seq { get; set; }

    }
}
