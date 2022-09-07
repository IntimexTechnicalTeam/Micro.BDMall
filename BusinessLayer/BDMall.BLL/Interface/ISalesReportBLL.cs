namespace BDMall.BLL
{
    public interface ISalesReportBLL:IDependency
    {

        Dictionary<string, HotSalesSummaryView> GetHotSalesProductList(int topMonthQty, int topWeekQty, int topDayQty, SortType sortType);

        Dictionary<string, List<OrderShowCaseSummary>> GetOrderShowList(OrderShowCond cond);

        List<ProductSummary> GetWaitingApproveProdLst(int getQty);
    }
}
