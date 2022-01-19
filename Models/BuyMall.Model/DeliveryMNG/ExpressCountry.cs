using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class ExpressCountry : BaseEntity<int>
    {
        public Guid ExpressId { get; set; }
        public int CountryId { get; set; }
    }
}