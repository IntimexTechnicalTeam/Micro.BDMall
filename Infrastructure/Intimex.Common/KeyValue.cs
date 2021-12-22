using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intimex.Common
{
    public class KeyValue
    {
        private string _id;
        public string Id {

            get
            {
                return _id.ToLower();
            }
            set { _id = value; }
        }
        public string Text { get; set; }
    }
}
