namespace BDMall.Model
{
    public class ExpressZoneCountry : BaseEntity<int>
    {
        public Guid ZoneId { get; set; }
        public int CountryId { get; set; }
    }
}