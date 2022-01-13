using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    public enum PaymentGateType
    {
        WECHAT,
        MPGS1,// MPGSMASTER,
        MPGS2,//MPGSUNION,
        STRIPE,
        PAYPAL,
        PAYME,
        ALIPAY,
        ALIPAYHK,
        ATOME
    }
}
