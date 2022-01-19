using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class ExpressPrice : BaseEntity<Guid>
    {
        public decimal WeightFrom { get; set; }

        public decimal WeightTo { get; set; }

        public decimal Price { get; set; }

        public Guid ZoneId { get; set; }

        public Guid RuleId { get; set; }

    }
}