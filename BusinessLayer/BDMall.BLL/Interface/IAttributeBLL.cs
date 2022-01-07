using BDMall.Domain;

using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IAttributeBLL:IDependency
    {

        List<KeyValue> GetInveAttribute();

        List<KeyValue> GetNonInveAttribute();

        List<KeyValue> GetAttrLayout();

        List<ProductAttributeValueDto> GetInveAttributeValueSummary();

        PageData<ProductAttributeDto> SearchAttribute(ProductAttributeCond attrCond);

        ProductAttributeDto GetAttribute(Guid id);

        ProductAttributeValueDto GetAttributeValue(Guid id);

        bool CheckAttrIsUsed(string ids);

        SystemResult DeleteAttribute(Guid[] ids);

        SystemResult Save(ProductAttributeDto attributeObj);

        List<AttributeObjectView> GetInvAttributeByCatId(Guid catId);

        List<AttributeObjectView> GetNonInvAttributeByCatId(Guid catId);

        List<AttributeObjectView> GetNonInvAttributeByProduct(Guid prodId);

        List<AttributeObjectView> GetInvAttributeByProduct(Guid prodId);

        
    }
}
