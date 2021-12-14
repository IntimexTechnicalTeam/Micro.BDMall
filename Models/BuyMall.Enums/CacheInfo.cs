using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    public enum CacheKey
    {
        Ticket=100,
        CurrentUser,
        InvtActualQty,
        SalesQty,
        InvtReservedQty,
        InvtHoldQty,

        CodeMaster,

        RefuseCountries,

        SupportCountries,

        Translations,

        PriceLimit,

        ShoppingCart,
    }
}
