using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public interface IInvTransactionDtlRepository :IDependency
    {
        PageData<InvFlow> GetInvTransDtlLst(InvFlowSrchCond cond);

        List<InvTransItemView> GetPurchaseItmLst(InvTransSrchCond condition);

        List<InvTransItemView> GetPurchaseReturnItmLst(InvTransSrchCond condition);
    }
}
