namespace BDMall.Repository
{
    public  interface IProductSkuRepository :IDependency
    {
        /// <summary>
        /// 根据三个属性确定一个唯一的失效或者有效的SKU
        /// </summary>
        /// <param name="prodCode"></param>
        /// <param name="attrValueId"></param>
        /// <returns></returns>
        List<ProductSku> GetSkuByAttrValueId(string prodCode, Guid attrValueId);

        ProductSku GetActiveSkuByAttrValueId(string prodCode, Guid attrValueId1, Guid attrValueId2, Guid attrValueId3);

        ProductSku GetSkuByAttrValueId(string prodCode, Guid attrValueId1, Guid attrValueId2, Guid attrValueId3);

        List<ProductSku> GenProduckSku(List<ProductAttr> attrs, string prodCode);
    }
}
