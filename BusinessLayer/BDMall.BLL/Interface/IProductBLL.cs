namespace BDMall.BLL
{
    public interface IProductBLL:IDependency
    {
        Dictionary<string, ClickRateSummaryView> GetClickRateView(int topMonthQty, int topWeekQty, int topDayQty);

        Dictionary<string, ClickRateSummaryView> GetSearchRateView(int topMonthQty, int topWeekQty, int topDayQty);

        PageData<ProductSummary> SearchBackEndProductSummary(ProdSearchCond cond);

        ProductEditModel GetProductInfo(Guid id);

        List<string> GetProductImages(Guid prodID);

        /// <summary>
        /// 检测Sku相关状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="attr1"></param>
        /// <param name="attr2"></param>
        /// <param name="attr3"></param>
        /// <param name="saleTime"></param>
        /// <returns></returns>
        Task<SystemResult<ProductCheck>> CheckSkuStateAsync(string code, Guid attr1, Guid attr2, Guid attr3, string saleTime);

        Task<SystemResult> CheckOnSaleAsync(string code);

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

        ProductSummary GenProductInfoBySkuId(Guid productId, Guid skuId);

        ProductSkuDto GetProductSku(Guid skuId);

        /// <summary>
        /// 获取SaleQty售罄的数据
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetSelloutSkus();

        Task<PageData<MicroProduct>> GetProductListAsync(ProductCond cond);

        Task<ProductDetailView> GetProductDetailAsync(string Code);

        Task<MicroProductDetail> GetMicroProductDetail(string Code);

        Task<List<string>> GetProductImages(Guid ProductId, string Code);

        Task CountClick(string code, bool isSearch);
    }
}
