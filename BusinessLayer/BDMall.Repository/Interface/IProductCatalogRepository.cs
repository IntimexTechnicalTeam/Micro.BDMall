using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public interface IProductCatalogRepository:IDependency
    {
        IQueryable<ProductCatalogDto> baseQuery();

        List<ProductCatalogDto> GetAllActiveCatalog();

        List<ProductCatalogDto> GetAllCatalog();

        ProductCatalogDto GetById(Guid id);
    }
}
