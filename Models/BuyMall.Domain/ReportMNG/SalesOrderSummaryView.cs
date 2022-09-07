namespace BDMall.Domain
{
    public class SalesOrderSummaryView
    {
        public List<string> TitleList { get; set; }

        public List<SalesOrderDetailView> SalesOrderDetailList { get; set; } = new List<SalesOrderDetailView>();
    }
}
