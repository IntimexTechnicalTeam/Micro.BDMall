namespace BDMall.Domain
{
    public class LastVersionProductView
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public List<MutiLanguage> Names { get; set; } = new List<MutiLanguage>();
        public Guid NameTransId { get; set; }
        public string SmallImage { get; set; }
        public Guid MerchantId { get; set; }
        public Guid CatalogId { get; set; }
    }
}
