using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    public enum EmailerStatus
    {
        Editting = 0,
        Processing = 1,
        Sendding = 2,
        Finish = 3,
        WaittingToSend = 4,
        Failed = 5

    }
}
