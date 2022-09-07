namespace BDMall.Domain
{
    public class ProductCheck
    {
        public bool IsOnSale { get; set; }

        public bool IsSaleOut { get; set; }

        /// <summary>
        /// 产品是否在发售时间
        /// </summary>
        public bool IsSelling { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsLimited { get; set; }

    }
}
