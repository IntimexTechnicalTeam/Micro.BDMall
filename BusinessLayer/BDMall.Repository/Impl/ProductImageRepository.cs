namespace BDMall.Repository
{
    public class ProductImageRepository : PublicBaseRepository, IProductImageRepository
    {
        public ProductImageRepository(IServiceProvider service) : base(service)
        {
        }

        public List<ProductImage> GetImageByProductId(Guid prodID)
        {
            List<ProductImage> list = baseRepository.GetList<ProductImage>(p =>  !p.IsDeleted && p.IsActive && p.ProductId == prodID).ToList();
            return list;
        }

        public List<ProductImage> GetImageByType(Guid prodID, ImageType type)
        {
            List<ProductImage> list = baseRepository.GetList<ProductImage>(p =>  !p.IsDeleted && p.IsActive && p.Type == type && p.ProductId == prodID).ToList();
            return list;
        }

        public ProductImage GetImageBySku(Guid prodID, Guid attrValue1, Guid attrValue2, Guid attrValue3, ImageType type)
        {
            var img = baseRepository.GetModel<ProductImage>(p => !p.IsDeleted && p.IsActive && p.Type == type && p.ProductId == prodID && p.AttrValue1 == attrValue1 && p.AttrValue2 == attrValue2 && p.AttrValue3 == attrValue3);
            return img;
        }

        public List<ProductImage> GetAdditionalImageBySkuImageId(Guid imageID)
        {

            List<ProductImage> list = (from s in baseRepository.GetList<ProductImage>()
                                       join a in baseRepository.GetList<ProductImage>() on new { a1 = s.ProductId, a2 = s.AttrValue1, a3 = s.AttrValue2, a4 = s.AttrValue3, a5 = ImageType.AdditionImage }
                                       equals new { a1 = a.ProductId, a2 = a.AttrValue1, a3 = a.AttrValue2, a4 = a.AttrValue3, a5 = a.Type }
                                       where  !s.IsDeleted && s.IsActive && !a.IsDeleted && a.IsActive
                                       && s.Id == imageID
                                       select a).OrderBy(d => d.Type).ToList();

            return list;
        }
    }
}
