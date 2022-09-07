namespace BDMall.Repository
{
    public interface IAttributeRepository:IDependency
    {
        PageData<ProductAttributeDto> SearchAttribute(ProductAttributeCond attrCond);

        ProductAttribute GetAttribute(Guid id);

        List<ProductAttribute> GetAttributeItemsByCatID(Guid catID);

        List<ProductAttribute> GetAttributeItemsByProductId(Guid prodID);
    }
}
