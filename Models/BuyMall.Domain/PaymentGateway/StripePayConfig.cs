using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class StripePayConfig : PayConfig
    {
        public string StripeSecretKey { get; set; }

        public string StripePublishableKey { get; set; }


    }
}
