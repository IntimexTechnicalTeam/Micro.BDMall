using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class ExpressZoneProvince : BaseEntity<int>
    {
        public Guid ZoneId { get; set; }
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
    }
}