namespace BDMall.BLL
{
    public interface IProductImageBLL:IDependency
    {
        List<ProductImageView> GetImageByProductId(Guid id);

        void SaveProductImage(ProductImageCondition cond);

        List<ProductImageView> GetProductSkuImageList(Guid prodID);

        List<ProductImageView> GetAdditionalImgs(Guid prodID);

        Task<SystemResult> SaveProductSkuImage(ProductSkuImage model);

        void SetDefaultImage(Guid prodID, Guid imageID);

        void DeleteImage(Guid imageID);

        string GetBigImgPath(string smallImgPath);
    }
}
