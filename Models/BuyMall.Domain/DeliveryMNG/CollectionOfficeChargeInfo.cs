namespace BDMall.Domain
{
    /// <summary>
    /// 柜台信息
    /// </summary>
    public class CollectionOfficeChargeInfo
    {


        public Guid Id { get; set; }

        /// <summary>
        /// 柜台Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 柜台名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 扣除免运费产品重量后得出的运费
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 没扣除免运费产品重量后得出的运费
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// 校驗碼
        /// </summary>
        public string Vcode { get; set; }
    }
}
