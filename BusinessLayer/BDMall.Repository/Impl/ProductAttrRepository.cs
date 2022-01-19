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
    public class ProductAttrRepository : PublicBaseRepository, IProductAttrRepository
    {
        public ProductAttrRepository(IServiceProvider service) : base(service)
        {
        }

        public List<ProductAttrDto> GetAttributeItemsMappByProductId(Guid prodID)
        {
            var attributes = (from a in baseRepository.GetList<ProductAttr>()
                              //join v in baseRepository.GetList<ProductAttrValue>() on a.Id equals v.ProdAttrId into vc
                              //from vv in vc.DefaultIfEmpty()
                              join aa in baseRepository.GetList<ProductAttribute>() on a.AttrId equals aa.Id
                              join at in baseRepository.GetList<Translation>() on new { a1 = aa.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = at.TransId, a2 = at.Lang }
                              into atc
                              from att in atc.DefaultIfEmpty()
                              where a.ProductId == prodID && !a.IsDeleted && a.IsActive
                              select new ProductAttrDto
                              {
                                  AttrName = att.Value ?? "",
                                  AttrId = a.AttrId,
                                  CatalogID = a.CatalogID,
                                  ClientId = a.ClientId,
                                  Id = a.Id,
                                  IsActive = a.IsActive,
                                  IsDeleted = a.IsDeleted,
                                  IsInv = a.IsInv,
                                  ProductId = prodID,
                                  CreateDate = a.CreateDate,
                                  CreateBy = a.CreateBy,
                                  Seq = a.Seq,
                                  UpdateBy = a.UpdateBy,
                                  UpdateDate = a.UpdateDate,                                 
                              }).Distinct().OrderBy(o => o.IsInv).ToList();

            attributes.ForEach(item =>
            {
                var attrValues = baseRepository.GetList<ProductAttrValue>(x => x.ProdAttrId == item.Id).ToList();
                item.AttrValues = AutoMapperExt.MapTo<List<ProductAttrValueDto>>(attrValues);
            });

            return attributes;
        }
    }
}
