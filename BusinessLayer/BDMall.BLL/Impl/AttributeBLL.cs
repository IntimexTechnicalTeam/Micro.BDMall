using BDMall.Domain;
using BDMall.Model;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.BLL
{
    public class AttributeBLL : BaseBLL, IAttributeBLL
    {
        public AttributeBLL(IServiceProvider services) : base(services)
        {
        }

        public List<KeyValue> GetInveAttribute()
        {          
            var inveAttributes = (from e in baseRepository.GetList<ProductAttribute>()
                                  join t in baseRepository.GetList<Translation>() on new { a1 = e.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                                  from tt in tc.DefaultIfEmpty() where e.IsActive && !e.IsDeleted && e.IsInvAttribute
                                  select new KeyValue
                                  {
                                      Id = e.Id.ToString(),
                                      Text = tt.Value
                                  }).ToList();

            return inveAttributes;

        }

        public List<KeyValue> GetNonInveAttribute()
        {           
            var nonInveAttributes = (from e in baseRepository.GetList<ProductAttribute>()
                                     join t in baseRepository.GetList<Translation>() on new { a1 = e.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                                     from tt in tc.DefaultIfEmpty()
                                     where e.IsActive && !e.IsDeleted && !e.IsInvAttribute
                                     select new KeyValue
                                     {
                                         Id = e.Id.ToString(),
                                         Text = tt.Value
                                     }).ToList();

            return nonInveAttributes;
        }

        public List<ProductAttributeValueDto> GetInveAttributeValueSummary()
        {
            var attrValues = (from d in UnitOfWork.DataContext.ProductAttributes
                              join v in UnitOfWork.DataContext.ProductAttributeValues on d.Id equals v.AttrId
                              join t in UnitOfWork.DataContext.Translations on new { a1 = v.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                              from tt in tc.DefaultIfEmpty()
                              where d.IsInvAttribute && !d.IsDeleted && d.IsActive
                              select new ProductAttributeValueDto
                              {
                                  AttrId = v.AttrId,
                                  Id = v.Id,
                                  Desc = tt.Value,
                                  Code = v.Code,
                                  MerchantId = v.MerchantId,
                                  Image = v.Image,
                                  DescTransId = v.DescTransId,

                              }).ToList();

            //foreach (var item in attrValues)
            //{
            //    item.MerchantName = baseRepository.GetModelById<Merchant>(item.MerchantId).n;
            //}
            return attrValues;
        }
    }
}
