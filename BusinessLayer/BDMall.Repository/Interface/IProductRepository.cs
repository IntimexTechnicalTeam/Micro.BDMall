namespace BDMall.Repository
{
    public interface IProductRepository :IDependency
    {
         Task UpdateProduct();

        List<Product> GetProductByAttrValueId(Guid attrValueId);

        List<Product> GetProductByAttrId(Guid attrId);

        PageData<ProductSummary> Search(ProdSearchCond cond);

        PageData<Product> SearchRelatedProduct(RelatedProductCond cond);

        List<Product> GetRelatedProduct(Guid id);

        LastVersionProductView GetLastVersionProductByCode(string prodCode);

        /// <summary>
        /// 根據產品編號的獲取當前產品的有效id
        /// </summary>
        /// <param name="prodCode"></param>
        /// <returns></returns>
        Guid? GetOnSaleProductId(string prodCode);

        List<LastVersionProductView> GetLastVersionProductLstByCode(List<string> prodCodeLst);

        Task<List<decimal>> GetProductAddPriceBySku(Guid id, Guid skuId);
    }
}
