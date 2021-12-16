using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IProductBLL:IDependency
    {
        Dictionary<string, ClickRateSummaryView> GetClickRateView(int topMonthQty, int topWeekQty, int topDayQty);

        Dictionary<string, ClickRateSummaryView> GetSearchRateView(int topMonthQty, int topWeekQty, int topDayQty);

        PageData<ProductSummary> SearchBackEndProductSummary(ProdSearchCond cond);
    }
}
