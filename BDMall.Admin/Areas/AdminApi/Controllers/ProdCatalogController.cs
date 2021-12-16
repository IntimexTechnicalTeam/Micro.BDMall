using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    public class ProdCatalogController : BaseApiController
    {
        IProductCatalogBLL productCatalogBLL;
        public ProdCatalogController(IComponentContext services) : base(services)
        {
            productCatalogBLL = Services.Resolve<IProductCatalogBLL>();
        }

        [HttpGet]
        public async Task<SystemResult> GetAllCatalog()
        {
            SystemResult result = new SystemResult();
            result.Succeeded = true;
            result.ReturnValue = productCatalogBLL.GetAllCatalog();
            return result;
        }

        [HttpGet]
        public SystemResult GetCatalogTree()
        {
            SystemResult result = new SystemResult();
            List<ProductCatalogEditModel> list = new List<ProductCatalogEditModel>();
            list = productCatalogBLL.GetCatalogTree(false);
            result.Succeeded = true;
            result.ReturnValue = list;
            return result;
        }

        public SystemResult GetActiveCatalogTree()
        {
            SystemResult result = new SystemResult();
            List<ProductCatalogEditModel> list = new List<ProductCatalogEditModel>();
            list = productCatalogBLL.GetCatalogTree(true);
            result.Succeeded = true;
            result.ReturnValue = list;
            return result;
        }

        [HttpGet]
        public SystemResult GetById(string id)
        {
            SystemResult result = new SystemResult();
            ProductCatalogEditModel catalog =new ProductCatalogEditModel();
            catalog = productCatalogBLL.GetCatalog(Guid.Parse(id));

            result.Succeeded = true;
            result.ReturnValue = catalog;

            return result;
        }

        [HttpGet]

        public SystemResult Delete(Guid id)
        {
            SystemResult result = new SystemResult();
            productCatalogBLL.DeleteCatalog(id);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 更新各個catalog的順序
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SystemResult> UpdateSeq(List<ProductCatalogEditModel> tree)
        {
            SystemResult result = new SystemResult();
            var list = TreeToList(tree);
            list = list.Where(p => p.IsChange == true).ToList();
            await productCatalogBLL.UpdateCatalogSeqAsync(list);
            result.Succeeded = true;
            return result;
        }

        [NonAction]
        private List<ProductCatalogEditModel> TreeToList(List<ProductCatalogEditModel> tree)
        {
            List<ProductCatalogEditModel> list = new List<ProductCatalogEditModel>();

            list = GenList(tree);

            return list;
        }

        [NonAction]
        private List<ProductCatalogEditModel> GenList(List<ProductCatalogEditModel> tree)
        {
            List<ProductCatalogEditModel> list = new List<ProductCatalogEditModel>();
            foreach (ProductCatalogEditModel item in tree)
            {
                //if (item.IsChange == true)
                //{
                list.Add(item);
                if (item.Children != null)
                {
                    list.AddRange(GenList(item.Children));
                }
                //}
            }

            return list;

        }

    }
}
