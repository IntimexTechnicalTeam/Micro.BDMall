namespace BDMall.Enums
{
    public enum OrderStatus
    {
        /// <summary>
        /// 收到訂單
        /// </summary>
        ReceivedOrder = 0,
        /// <summary>
        /// 己確認付款
        /// </summary>
        PaymentConfirmed = 1,

        /// <summary>
        /// 處理中
        /// </summary>
        Processing = 2,

        /// <summary>
        /// 已安排送貨
        /// </summary>
        DeliveryArranged = 3,

        /// <summary>
        /// 訂單完成
        /// </summary>
        OrderCompleted = 4,
        /// <summary>
        /// 用户取消订单
        /// </summary>
        SCancelled = 5,
        /// <summary>
        /// 用户重寄取消订单
        /// </summary>
        SRCancelled = 6,
        /// <summary>
        /// 取消过期订单
        /// </summary>
        ECancelled = 7
        /// <summary>
        /// 退貨
        /// </summary>
        //ReturnsForCancell=6

    }
}
