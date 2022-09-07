namespace BDMall.Domain
{
    public class InvSummary: InvSummaryView
    {
        public int HoldTotalQty { get; set; }
        public List<InvSummaryDetl> InventoryDetailList { get; set; } = new List<InvSummaryDetl>();
    }
}
