using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class AtomeConfig : PayConfig
    {
        public string ApiKey { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
    }
}
