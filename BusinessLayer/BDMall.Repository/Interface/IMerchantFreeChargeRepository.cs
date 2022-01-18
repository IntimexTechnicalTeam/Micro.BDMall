using BDMall.Domain;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public interface IMerchantFreeChargeRepository : IDependency
    {
        List<MerchantFreeCharge> GetByMerchantId(Guid id);
        MerchantFreeChargeView GetMerchantFreeChargeInfo(Guid id, List<string> shipCodes);

        List<MerchantFreeCharge> GetByCode(string code);
    }
}
