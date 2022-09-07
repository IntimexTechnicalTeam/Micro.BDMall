namespace BDMall.Domain
{
    public class CouponInfo : DiscountInfo
    {
        public string MerchantName { get; set; }

        public CouponStatus CouponStatus { get; set; }

        public bool IsUsed { get; set; }
        public DateTime? UsedDate { get; set; }

    }
}
