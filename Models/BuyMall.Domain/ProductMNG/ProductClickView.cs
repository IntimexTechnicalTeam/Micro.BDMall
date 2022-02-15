using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductClickView
    {
        public Queue<string> ClickQueue { get; set; } = new Queue<string>();

        public Queue<string> SearchQueue { get; set; } = new Queue<string>();

        public DateTime UpdateDate { get; set; }
    }
}
