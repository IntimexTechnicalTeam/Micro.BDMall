namespace BDMall.Domain
{
    public class SimpleCurrency
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 與基本貨幣的兌換率
        /// </summary>
        public decimal ExchangeRate { get; set; }

        public bool IsDefaultCurrency => Code == "HKD" ? true : false;
    }
}
