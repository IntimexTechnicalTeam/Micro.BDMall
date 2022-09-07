namespace BDMall.Repository
{
    public interface IReturnOrderRepository:IDependency
    {
        List<ReturnOrder> GetReturnOrders(ReturnOrderCondition cond);
    }
}
