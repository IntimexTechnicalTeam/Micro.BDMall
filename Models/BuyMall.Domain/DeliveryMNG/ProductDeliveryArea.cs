namespace BDMall.Domain
{
    public class ProductDeliveryArea
    {
        public Guid ProductId { get; set; }

        public string Code { get; set; }

        public Country Country { get; set; }
    }
}
