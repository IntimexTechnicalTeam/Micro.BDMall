namespace BDMall.Domain
{
    public class ZoneInfo
    {
        public ExpressZoneDto zone { get; set; }
        public List<exCounProvince> exCounProvince { get; set; }
        public int[] exCountry { get; set; }
    }
    public class exCounProvince
    {
        public int country { get; set; }
        public string[] province { get; set; }
    }
}
