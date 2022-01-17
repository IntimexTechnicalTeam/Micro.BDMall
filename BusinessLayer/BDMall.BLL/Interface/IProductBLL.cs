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

        PageData<ProductSummary> SearchRelatedProduct(RelatedProductCond cond);

        PageData<ProductSummary> SearchProductList(ProdSearchCond cond);

        List<ProductSummary> GetRelatedProduct(Guid id);

        void AddRelatedProduct(List<string> prodCodes, Guid originalId);

        void DeleteRelatedProduct(Guid prodId, List<string> prodCodes);

        Task ProductLogicalDelete(List<string> prodIDs);

        Task ActiveProducts(List<string> ids);

        Task DisActiveProducts(List<string> ids);

        SystemResult ApplyApprove(Guid id);

        Task TurndownProduct(Guid prodID, string reason);

        ProductSummary GetProductSummary(Guid id, Guid skuId);

        ProductSkuDto GetProductSku(Guid skuId);
    }
}
