namespace BDMall.Domain
{
    public class MerchantShipMethodMappingView
    {
        public Guid MerchantId { get; set; }

        public List<MerchantShipMethodView> MerchantShipMethods { get; set; } = new List<MerchantShipMethodView>();


    }
}
