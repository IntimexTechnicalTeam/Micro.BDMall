using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class CountryZoneView
    {
        public int CountryId { get; set; }

        public List<KeyValue> Province { get; set; }
    }
}
