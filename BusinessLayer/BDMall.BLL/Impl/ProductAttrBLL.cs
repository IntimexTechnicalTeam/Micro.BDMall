using BDMall.Domain;
using BDMall.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public  class ProductAttrBLL:BaseBLL, IProductAttrBLL
    {
        public IProductAttrRepository productAttrRepository;

        public ProductAttrBLL(IServiceProvider services) : base(services)
        {
            productAttrRepository = Services.Resolve<IProductAttrRepository>();
        }

        public List<AttributeObjectView> GetInvAttributeByProductMap(Guid prodId)
        {
            List<AttributeObjectView> list = new List<AttributeObjectView>();

            var attrs = productAttrRepository.GetAttributeItemsMappByProductId(prodId).Where(p => p.IsInv == true).ToList();
            if (attrs != null && attrs.Any())
            {
                foreach (var item in attrs)
                {
                    AttributeObjectView obj = new AttributeObjectView();
                    obj.Id = item.AttrId;
                    obj.Desc = "";
                    obj.SubItems = item.AttrValues.Where(x => !x.IsDeleted)
                                        .Select(s => new AttributeValueView
                                        {
                                            Id = item.AttrId.ToString(),
                                            Text = s.AttrValueId.ToString(),
                                            Price = s.AdditionalPrice
                                        })
                                        .ToList();
                    list.Add(obj);
                }
            }
            return list;
        }

        public List<AttributeObjectView> GetNonInvAttributeByProductMap(Guid prodId)
        {
            List<AttributeObjectView> list = new List<AttributeObjectView>();

            var attrs = productAttrRepository.GetAttributeItemsMappByProductId(prodId).Where(p => p.IsInv == false).OrderBy(o=>o.Seq).ToList();

            if (attrs != null && attrs.Any())
            {
                foreach (var item in attrs)
                {                 
                    AttributeObjectView obj = new AttributeObjectView();
                    obj.Id = item.AttrId;
                    obj.Desc = "";                   
                    obj.SubItems = item.AttrValues.Where(x => !x.IsDeleted)
                                        .Select(s => new AttributeValueView
                                        {
                                            Id = item.AttrId.ToString(),
                                            Text = s.AttrValueId.ToString(),
                                            Price = s.AdditionalPrice
                                        })
                                        .ToList();
                    list.Add(obj);
                }
            }

            return list;
        }

        //private List<AttributeObjectView> GenAttributeObjectView(List<ProductAttribute> dbAttrs)
        //{
        //    List<AttributeObjectView> list = new List<AttributeObjectView>();

        //    if (dbAttrs != null && dbAttrs.Any())
        //    {
        //        var attrs = AutoMapperExt.MapTo<List<ProductAttributeDto>>(dbAttrs);
        //        foreach (var item in attrs)
        //        {
        //            var modelAttr = GenProductAttribute(item);
        //            AttributeObjectView obj = new AttributeObjectView();
        //            obj.Id = modelAttr.Id;
        //            obj.Desc = modelAttr.Desc;
        //            obj.SubItems = modelAttr.AttributeValues.Where(x => !x.IsDeleted && (x.MerchantId == CurrentUser.MerchantId || x.MerchantId == Guid.Empty))
        //                                        .Select(s => new AttributeValueView { Id = s.Id.ToString(), Text = s.Desc }).ToList();

        //            list.Add(obj);
        //        }
        //    }

        //    return list;
        //}
    }
}
