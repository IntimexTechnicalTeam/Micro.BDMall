namespace BDMall.Repository
{
    public interface IProductImageRepository:IDependency
    {

        List<ProductImage> GetImageByProductId(Guid prodID);

        List<ProductImage> GetImageByType(Guid prodID, ImageType type);

        ProductImage GetImageBySku(Guid prodID, Guid attrValue1, Guid attrValue2, Guid attrValue3, ImageType type);

        List<ProductImage> GetAdditionalImageBySkuImageId(Guid imageID);
    }
}
