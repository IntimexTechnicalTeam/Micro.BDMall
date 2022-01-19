using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ZoneInfo
    {
        public ExpressZoneDto zone { get; set; }
        public List<exCounProvince> exCounProvince { get; set; }
        public int[] exCountry { get; set; }
    }
    public class exCounProvince
    {
        public int country { get; set; }
        public string[] province { get; set; }
    }
}
