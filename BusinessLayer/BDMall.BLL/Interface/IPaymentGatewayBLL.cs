using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IPaymentGatewayBLL : IDependency
    {
        PayConfig GetConfig(PaymentGateType gateway);

        bool SaveOrUpdateConfig(PayConfig config);
    }
}
