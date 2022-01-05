using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class RnpSubmitDataGroupView
    {
        public Guid? AnswerId { get; set; }
        public string Result { get; set; }

        public int? Qty { get; set; }

        public decimal? Price { get; set; }
    }
}
