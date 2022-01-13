using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
