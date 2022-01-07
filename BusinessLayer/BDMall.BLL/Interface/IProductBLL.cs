using BDMall.Domain;
using BDMall.Enums;
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

        ProductEditModel GetProductInfo(Guid id);

        List<string> GetProductImages(Guid prodID);

        SystemResult CheckTimePriceByCode(string code, Guid MerchantId);

        ProductDto SaveProduct(ProductEditModel product);

        Task UpdateCache(string Code, ProdAction action);

        Task<List<string>> CopyProductImageToPath(ProductEditModel product);

        Task CreateDefaultImage(ProductEditModel product);
    }
}
