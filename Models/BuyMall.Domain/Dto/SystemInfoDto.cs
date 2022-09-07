namespace BDMall.Domain
{
    public class SystemInfoDto
    {
        /// <summary>
        /// 多语言
        /// </summary>
        public List<SystemLang> langs { get; set; } = new List<SystemLang>();

        /// <summary>
        /// 币种
        /// </summary>
        public List<SimpleCurrency> simpleCurrencies { get; set; } = new List<SimpleCurrency>();

        /// <summary>
        /// 摆档商城信息
        /// </summary>
        public MallConfig mallConfig { get; set; } = new MallConfig();
    }
}
