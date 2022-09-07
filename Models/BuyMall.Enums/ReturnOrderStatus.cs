namespace BDMall.Enums
{
    /// <summary>
    /// 退換單狀態
    /// </summary>
    public enum ReturnOrderStatus
    {
        /// <summary>
        /// 申請中
        /// </summary>
        Apply = 1,
        /// <summary>
        /// 通過申請
        /// </summary>
        Pass = 2,
        /// <summary>
        /// 拒絕申請
        /// </summary>
        Turndown = 3,
        /// <summary>
        /// 已安排退貨
        /// </summary>
        Delivery = 4
    }
}
