using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class PayAccount
    {
        public int Id { get; set; }

        public string PaymentAccount { get; set; }

        public bool PayClose { get; set; }

    }
}
