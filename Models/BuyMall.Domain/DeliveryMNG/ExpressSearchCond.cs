namespace BDMall.Domain
{
    public class ExpressSearchCond
    {
        public string CCode { get; set; }
        public string TCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid MerchantId { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
