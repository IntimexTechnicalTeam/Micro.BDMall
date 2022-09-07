namespace BDMall.Repository
{
    public interface IInvTransactionDtlRepository :IDependency
    {
        PageData<InvFlow> GetInvTransDtlLst(InvFlowSrchCond cond);

        List<InvTransItemView> GetPurchaseItmLst(InvTransSrchCond condition);

        List<InvTransItemView> GetPurchaseReturnItmLst(InvTransSrchCond condition);
    }
}
