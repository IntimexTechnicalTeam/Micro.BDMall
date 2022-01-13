using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [AdminApiAuthorize(Module = ModuleConst.ProductModule)]
    [ApiController]
    public class ProdImageController : BaseApiController
    {
        public IProductImageBLL productImageBLL;

        public ProdImageController(IComponentContext services) : base(services)
        {
            productImageBLL = Services.Resolve<IProductImageBLL>();
        }


        public List<ProductImageView> GetSkuProductImageList(Guid prodID)
        {
            List<ProductImageView> imgs = productImageBLL.GetProductSkuImageList(prodID);

            return imgs;
        }

        /// <summary>
        /// 根據產品SKU獲取產品的Additional圖片信息
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        [HttpGet]

        public List<ProductImageView> GetAdditionalImg(Guid prodID)
        {
            List<ProductImageView> imgs = productImageBLL.GetAdditionalImgs(prodID);
            return imgs;
        }

        [HttpGet]
        public async Task<SystemResult> SaveProductSkuImage(Guid prodID, Guid attr1, Guid attr2, Guid attr3, string path)
        {
            var result = new SystemResult();
            var input = new ProductSkuImage { ProductId = prodID , Attr1 = attr1, Attr2 = attr2, Attr3 = attr3,Path = path };
            result= await productImageBLL.SaveProductSkuImage(input);
            return result;
        }

        [HttpGet]
        public SystemResult SetDefaultImage(Guid prodID, Guid imageID)
        {
            var result = new SystemResult();
            productImageBLL.SetDefaultImage(prodID, imageID);
            result.Succeeded = true;
            return result;
        }

        [HttpGet]
        public SystemResult DeleteImage(Guid imgID)
        {
            SystemResult result = new SystemResult();
            productImageBLL.DeleteImage(imgID);
            result.Succeeded = true;
            return result;
        }
    }
}
