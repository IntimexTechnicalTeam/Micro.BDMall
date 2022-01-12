using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class PaymentMethodView
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }
        public string Code { get; set; }

        /// <summary>
        /// 支付方式服務費率
        /// </summary>
        public decimal ServRate { get; set; }
    }
}
