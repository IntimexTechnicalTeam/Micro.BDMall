using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ClickOrSearchDto
    {
        public string ProductCode { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int ClickQty { get; set; }

        public int SearchClickQty { get; set; }

        public Translation ProductName { get; set; }
    }
}
