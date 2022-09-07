namespace BDMall.Model
{
    public class ExpressZoneProvince : BaseEntity<int>
    {
        public Guid ZoneId { get; set; }
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
    }
}