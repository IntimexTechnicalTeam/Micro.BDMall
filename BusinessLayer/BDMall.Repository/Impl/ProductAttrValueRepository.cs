using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Repository
{
    public class ProductAttrValueRepository : PublicBaseRepository, IProductAttrValueRepository
    {
        public ProductAttrValueRepository(IServiceProvider service) : base(service)
        {
        }

        public bool CheckHasInvRecordByAttrValueId(Guid attrValueId)
        {          
            var record = from d in baseRepository.GetList<ProductSku>()
                            join i in baseRepository.GetList<Inventory>() on d.Id equals i.Sku
                            where i.IsActive && !i.IsDeleted && i.MerchantId == CurrentUser.MerchantId
                            && (d.AttrValue1 == attrValueId || d.AttrValue2 == attrValueId || d.AttrValue3 == attrValueId)
                            select i;

            return record.Any();
        }
    }
}
