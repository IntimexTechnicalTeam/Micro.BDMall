namespace BDMall.Domain
{
    public class ClickRateSummaryView
    {
        public List<string> TitleList { get; set; } = new List<string>();

        public List<ClickRateDetailView> ClickRateDetailList { get; set; } = new List<ClickRateDetailView>();
    }
}
