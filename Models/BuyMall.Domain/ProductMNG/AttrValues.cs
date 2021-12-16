using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class AttrValues
    {
        
        public int Id { get; set; }
        public int Type { get; set; }

        public List<int> Vals { get; set; } = new List<int>();

    }

}
