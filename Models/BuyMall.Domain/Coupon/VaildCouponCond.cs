namespace BDMall.Domain
{
    public class VaildCouponCond
    {
        public Guid MerchantId { get; set; }

        /// <summary>
        /// 貨物總價
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 貨物總運費
        /// </summary>
        public decimal TotalDeliveryCharge { get; set; }


        //public List<string> CouponIds { get; set; }


    }
}
