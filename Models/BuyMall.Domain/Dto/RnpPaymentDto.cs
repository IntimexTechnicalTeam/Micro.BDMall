using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class RnpPaymentDto
    {
        public Guid Id { get; set; }
        public string BankAccount { get; set; }

        public Guid FormId { get; set; }

        public string PayGateway { get; set; }

    }
}
