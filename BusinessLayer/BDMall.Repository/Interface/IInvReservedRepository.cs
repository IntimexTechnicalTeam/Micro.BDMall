namespace BDMall.Repository
{
    public interface IInvReservedRepository:IDependency
    {
        List<InventoryReserved> GetInvReservedLst(InventoryReserved cond);
    }
}
