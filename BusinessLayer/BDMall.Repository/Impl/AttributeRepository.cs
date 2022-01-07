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
    public class AttributeRepository : PublicBaseRepository, IAttributeRepository
    {
        public AttributeRepository(IServiceProvider service) : base(service)
        {

        }

        public PageData<ProductAttributeDto> SearchAttribute(ProductAttributeCond attrCond)
        {
            var result = new PageData<ProductAttributeDto>();

            var query = (from a in baseRepository.GetList<ProductAttribute>()
                         join t in baseRepository.GetList<Translation>() on new { a1 = a.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                         from tt in tc.DefaultIfEmpty()
                         where a.IsInvAttribute == attrCond.IsInv && !a.IsDeleted && a.IsActive
                         //&& tt.Value.Contains(attrCond.Desc)
                         select new ProductAttributeDto
                         {
                             Id = a.Id,
                             Code = a.Code,
                             DescTransId = a.DescTransId,
                             IsInvAttribute = a.IsInvAttribute,
                             Layout = a.Layout,
                             CreateDate = a.CreateDate,
                             CreateBy = a.CreateBy,
                             IsActive = a.IsActive,
                             IsDeleted = a.IsDeleted,
                             UpdateBy = a.UpdateBy,
                             UpdateDate = a.UpdateDate,
                             Value = tt.Value,
                             Desc = tt.Value,
                         });
            
            if (!attrCond.Desc.IsEmpty())
                query = query.Where(p => p.Value.Contains(attrCond.Desc));

            if (attrCond.PageInfo.SortName.IsEmpty())
                query = query.OrderByDescending(o => o.CreateDate);
            else            
                query = query.SortBy(attrCond.PageInfo.SortName, attrCond.PageInfo.SortOrder.ToUpper().ToEnum<SortType>());
            
            result.TotalRecord = query.Count();
            var data = query.Skip(attrCond.PageInfo.Offset).Take(attrCond.PageInfo.PageSize).ToList();

            result.Data = data;
            return result;
        }

        public ProductAttribute GetAttribute(Guid id)
        {
            ProductAttribute result = baseRepository.GetModel<ProductAttribute>(p => p.Id == id && !p.IsDeleted && p.IsActive);             
            return result;
        }

        public List<ProductAttribute> GetAttributeItemsByCatID(Guid catID)
        {
            var attributes = (from e in baseRepository.GetList<ProductAttribute>()
                              join c in baseRepository.GetList<ProductCatalogAttr>() on e.Id equals c.AttrId
                              where c.CatalogId == catID && !e.IsDeleted && e.IsActive && c.IsActive && !c.IsDeleted
                              select e).ToList();

            return attributes;
        }

        public List<ProductAttribute> GetAttributeItemsByProductId(Guid prodID)
        {
           var attributes = (from e in  baseRepository.GetList<ProductAttribute>()
                                  join c in baseRepository.GetList<ProductAttr>() on e.Id equals c.AttrId
                                  where c.ProductId == prodID && !e.IsDeleted
                                  && e.IsActive && !c.IsDeleted && c.IsActive
                                  //&& e.IsInvAttribute == true
                                  select e).ToList();

                return attributes;
           
        }




    }
}
