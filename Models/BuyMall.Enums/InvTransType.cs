namespace BDMall.Enums
{
    public enum InvTransType
    {
        Unknown = -1,
        /// <summary>
        /// 採購
        /// </summary>
        Purchase = 1,
        /// <summary>
        /// 調撥
        /// </summary>
        Relocation = 2,
        /// <summary>
        /// 採購退回
        /// </summary>
        PurchaseReturn = 3,
        /// <summary>
        /// 銷售出貨
        /// </summary>
        SalesShipment = 4,
        /// <summary>
        /// 銷售退回
        /// </summary>
        SalesReturn = 5,
        /// <summary>
        /// 發貨返回
        /// </summary>
        DeliveryReturn = 6
    }
}
