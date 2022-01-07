using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDMall.Model
{
    public class MerchantFreeCharge : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid MerchantId { get; set; }

        [StringLength(100)]
        [Column(Order = 4, TypeName = "varchar")]
        public string ProductCode { get; set; }

        [StringLength(10)]
        [Column(Order = 5, TypeName = "varchar")]
        public string ShipCode { get; set; }


    }
}
