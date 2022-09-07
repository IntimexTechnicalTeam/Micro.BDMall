namespace BDMall.Domain
{
    public class ExpressCompanyDto : BaseDto
    {
        public Guid Id { get; set; }
        public Guid NameTransId { get; set; }
        public string CCode { get; set; }
        public string TCode { get; set; }
        public string Code { get; set; }
        public bool UseApi { get; set; }
        public string Name { get; set; }
        public List<MutiLanguage> Names { get; set; }
        public List<int> CountryIds { get; set; }


    }
}
