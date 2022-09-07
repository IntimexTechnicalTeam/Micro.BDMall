namespace BDMall.Domain
{
    /// <summary>
    /// 產品提成相關數據
    /// </summary>
    public class ProductCommissionView
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public decimal? CMVal { get; set; }
        public decimal? CMRate { get; set; }
        public ProdCommissionType CMCalType { get; set; } = ProdCommissionType.MerchInheriting;
    }
}
