namespace BDMall.Domain
{
    public class CityDto : BaseDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Name_e { get; set; }
        public string Name_c { get; set; }
        public string Name_s { get; set; }
        public string Name_j { get; set; }

        public int ProvinceId { get; set; }
        public List<MutiLanguage> Names { get; set; }


    }
}
