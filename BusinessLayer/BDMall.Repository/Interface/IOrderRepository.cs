namespace BDMall.Repository
{
    public interface IOrderRepository:IDependency
    {
        PageData<OrderView> GetSimpleOrderByPage(OrderCondition cond);

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="order"></param>
        void UpdateOrderStatus(Order order);
    }
}
