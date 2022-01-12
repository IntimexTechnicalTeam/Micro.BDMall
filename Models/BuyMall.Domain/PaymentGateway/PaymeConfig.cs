using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class PaymeConfig : PayConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SigningKeyId { get; set; }
        public string SigningKey { get; set; }

        public string Url { get; set; }

        public string Ver { get; set; }
    }
}
