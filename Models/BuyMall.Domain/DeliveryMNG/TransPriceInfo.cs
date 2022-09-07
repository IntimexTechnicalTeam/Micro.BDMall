namespace BDMall.Domain
{
    public class TransPriceInfo
    {
        public decimal Weight { get; set; }
        public List<ExpressPrice> zoneCharge { get; set; }
    }
}
