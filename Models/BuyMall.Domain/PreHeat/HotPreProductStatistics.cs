namespace BDMall.Domain
{
    public class HotPreProductStatistics
    {
        public string Code { get; set; }

        //public string ProductId { get; set; }

        public decimal Score { get; set; }

        public int PurchaseCounter { get; set; }

        public int VisitCounter { get; set; }

        public int SearchCounter { get; set; }
    }
}
