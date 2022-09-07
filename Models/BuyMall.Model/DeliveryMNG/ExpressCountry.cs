namespace BDMall.Model
{
    public class ExpressCountry : BaseEntity<int>
    {
        public Guid ExpressId { get; set; }
        public int CountryId { get; set; }
    }
}