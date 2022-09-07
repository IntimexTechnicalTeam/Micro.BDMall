namespace BDMall.Domain
{
    public class HotSalesSummaryView
    {
        public List<string> TitleList { get; set; } = new List<string>();

        public List<HotSalesDetailView> HotSalesDetailList { get; set; } = new List<HotSalesDetailView>();
    }
}
