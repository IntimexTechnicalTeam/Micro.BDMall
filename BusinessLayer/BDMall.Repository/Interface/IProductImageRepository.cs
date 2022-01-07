using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public interface IProductImageRepository:IDependency
    {

        List<ProductImage> GetImageByProductId(Guid prodID);

        List<ProductImage> GetImageByType(Guid prodID, ImageType type);
    }
}
