using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IProductCatalogBLL:IDependency
    {
        List<ProductCatalogSummaryView> GetAllCatalog();

        List<ProductCatalogEditModel> GetCatalogTree(bool isActive);

        ProductCatalogEditModel GetCatalog(Guid catId);

        void DeleteCatalog(Guid id);

        Task<SystemResult> SaveCatalog(ProductCatalogEditModel productCatalog);

        Task UpdateCatalogSeqAsync(List<ProductCatalogEditModel> list);

        Task<SystemResult> DisActiveCatalogAsync(Guid catId);

        Task<SystemResult> ActiveCatalogAsync(Guid catId);

        string GetCatalogPath(Guid catID);

        Task<SystemResult> GetCatalogAsync();
    }
}
