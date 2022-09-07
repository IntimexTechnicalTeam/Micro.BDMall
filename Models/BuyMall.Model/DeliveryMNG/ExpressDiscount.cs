namespace BDMall.Model
{
    public class ExpressDiscount : BaseEntity<Guid>
    {
        //public string Name { get; set; }
        public Guid ExpressId { get; set; }

        public decimal DiscountMoney { get; set; }

        public decimal DiscountPercent { get; set; }
        public bool IsPercent { get; set; }

        public Guid MerchantId { get; set; }

    }
}