using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class ExpressCompany : BaseEntity<Guid>
    {

        public Guid NameTransId { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
        public decimal Discount { get; set; }
       
    }
}