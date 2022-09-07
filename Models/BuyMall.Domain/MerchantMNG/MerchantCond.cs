namespace BDMall.Domain
{
    public class MerchantCond : PageInfo
    {
        public string Name { get; set; } = "";

        public bool ShowPass { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? IsActiveCond { get; set; }
    }
}
