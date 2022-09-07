namespace BDMall.Domain
{
    public class MerchantFreeChargeProductView
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public bool IsDeleted { get; set; }
    }
}
