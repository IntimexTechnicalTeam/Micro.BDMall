using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class RnpSubmit : BaseEntity<Guid>
    {

        public Guid FormId { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalAmount { get; set; }

        public bool IsPayed { get; set; }

        public string GatewayResponse { get; set; }

        [StringLength(50)]
        public string UpdatePayedBy { get; set; }

        [StringLength(50)]
        public string Signature { get; set; }
    }
}
