namespace BDMall.Repository
{
    public interface IDealProductQtyRepository : IDependency
    {
        Task<int> UpdateQtyWhenPurchasing(Guid SkuId, int InvtActualQty, int SalesQty, Guid Id);

        Task<int> UpdateQtyWhenReturn(Guid SkuId, int InvtActualQty, int SalesQty, Guid Id);

        Task<int> UpdateQtyWhenAddToCart(Guid SkuId, int InvtHoldQty, int SalesQty, Guid Id);

        Task<int> UpdateQtyWhenDeleteCart(Guid SkuId, int InvtHoldQty, int SalesQty, Guid Id);

        Task<int> UpdateQtyWhenModifyCart(Guid SkuId, int InvtHoldQty, int SalesQty, Guid Id);

        Task<int> UpdateQtyWhenPay(Guid SkuId, int InvtReservedQty, int SalesQty, int InvtHoldQty, Guid Id);

        Task<int> UpdateQtyWhenDeliveryArranged(Guid SkuId, int InvtReservedQty, int InvtActualQty, int SalesQty, Guid Id);

        Task<int> UpdateQtyWhenOrderCancel(Guid SkuId, int InvtReservedQty, int SalesQty, Guid Id);

        Task<int> UpdateQtyWhenPayTimeOut(Guid SkuId, int InvtHoldQty, int SalesQty, Guid Id);

        Task<int> UpdateProductyQty(IEnumerable<PreProductQty> dataSource);
    }
}
