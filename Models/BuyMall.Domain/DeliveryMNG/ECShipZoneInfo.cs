namespace BDMall.Domain
{
    public class ECShipZoneInfo
    {
        /// <summary>
        /// 快递ID
        /// </summary>
        public Guid ExpressCompanyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CountryCode { get; set; }

        public string ZoneCode { get; set; }

        /// <summary>
        /// 快递名称ID
        /// </summary>
        public Guid ExpressCompanyNameId { get; set; }

        public string ExpressCompanyName { get; set; }
    }
}
