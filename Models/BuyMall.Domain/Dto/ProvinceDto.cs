using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class ProvinceDto
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
        public string Name_e { get; set; }
        public string Name_c { get; set; }
        public string Name_s { get; set; }
        public string Name_j { get; set; }

        public int CountryId { get; set; }


        public List<MutiLanguage> Names { get; set; }

        public List<CityDto> Cities { get; set; }

        public virtual void Validate()
        {
            if (Cities?.Any() ?? false)
            { 
                var flag = Cities.Any(x=>x.Code.IsEmpty());
                if (flag) throw new BLException("Code is Required");
            }
        }
    }
}
