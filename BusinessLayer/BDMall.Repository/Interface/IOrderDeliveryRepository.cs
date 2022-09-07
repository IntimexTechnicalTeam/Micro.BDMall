namespace BDMall.Repository
{
    public interface IOrderDeliveryRepository:IDependency
    {
        /// <summary>
        /// 更新OrderDelivery状态
        /// </summary>
        /// <param name="delivery"></param>
        void UpdateOrderDeliveryStatus(OrderDelivery delivery);

        //void UpdateOrderDeliveryArrangedStatus(OrderDelivery delivery);

        void UpdateOrderDeliveryDetail(OrderDeliveryDetailDto deliveryDetail);

        void UpdateOrderDeliveryTrackingNo(OrderDelivery delivery);
        void UpdateDeliverySendBackCount(OrderDelivery delivery);

        List<OrderDelivery> Search(Guid orderId, Guid merchantId);

        void UpdateDeliveryInfo(OrderDeliveryInfo delivery);
    }
}
