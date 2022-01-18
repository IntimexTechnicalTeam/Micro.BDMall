using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class CountryVaildInfo
    {
        public CountryVaildInfo()
        {
            this.CountryId = 0;
            this.AddressId = Guid.Empty;
            this.ProductIds = new List<string>();
        }
        public int CountryId { get; set; }

        public Guid AddressId { get; set; }

        public List<string> ProductIds { get; set; }
    }
}
