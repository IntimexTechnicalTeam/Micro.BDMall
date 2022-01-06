using BDMall.Domain;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public interface IProductRepository :IDependency
    {
         Task UpdateProduct();

        List<Product> GetProductByAttrValueId(Guid attrValueId);

        List<Product> GetProductByAttrId(Guid attrId);

        PageData<ProductSummary> Search(ProdSearchCond cond);
    }
}
