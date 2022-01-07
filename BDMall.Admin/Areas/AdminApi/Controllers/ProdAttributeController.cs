using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
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
    public class ProdAttributeController : BaseApiController
    {
        IAttributeBLL attributeBLL;
        public ProdAttributeController(IComponentContext services) : base(services)
        {
            attributeBLL = Services.Resolve<IAttributeBLL>();
        }

        /// <summary>
        /// 获取库存属性的下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValue> GetInveAttribute()
        {
            List<KeyValue> list = new List<KeyValue>();
            list = attributeBLL.GetInveAttribute();
            return list;
        }

        [HttpGet]
        public List<KeyValue> GetNonInveAttribute()
        {
            List<KeyValue> list = new List<KeyValue>();
            list = attributeBLL.GetNonInveAttribute();
            return list;
        }

        [HttpPost]
        public PageData<ProductAttributeDto> Search(ProductAttributeCond cond)
        {
            var result =attributeBLL.SearchAttribute(cond);
            return result;       
        }

        [HttpGet]
        public ProductAttributeDto GetById(string id)
        {
            var attr = attributeBLL.GetAttribute(Guid.Parse(id));
            return attr;
        }

        [HttpGet]
        public ProductAttributeValueDto GetItemById(string id)
        {
            var  attr = attributeBLL.GetAttributeValue(Guid.Parse(id));
            return attr;
        }

        /// <summary>
        /// 檢查自定義屬性是否比使用
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public bool CheckAttrIsUsed(string idList)
        {         
            return attributeBLL.CheckAttrIsUsed(idList);
        }

        /// <summary>
        /// 刪除屬性
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpGet]
        public SystemResult Delete(string idList)
        {
            var strIdList = idList.Split(',');         
            var ids = strIdList.ToList().Select(s=>Guid.Parse(s)).ToArray();
            SystemResult result = new SystemResult();

            attributeBLL.DeleteAttribute(ids);
            result.Succeeded = true;
            return result;
        }

        [HttpPost]
        public async Task<SystemResult> Save([FromForm]ProductAttributeDto attributeObj)
        {
            SystemResult result = new SystemResult();
            attributeObj.Validate();
            result = attributeBLL.Save(attributeObj);
            return result;
        }

        /// <summary>
        /// 通过产品目录获取库存属性的下拉框对象
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<AttributeObjectView> GetInvAttributeByCatId(Guid catId)
        {
            var list = attributeBLL.GetInvAttributeByCatId(catId);
            return list;
        }

        /// <summary>
        /// 通过产品目录获取非库存属性的下拉框对象
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<AttributeObjectView> GetNonInvAttributeByCatId(Guid catId)
        {
            var list  = attributeBLL.GetNonInvAttributeByCatId(catId);
            return list;
        }

        /// <summary>
        /// 通过产品ID获取产品下的属性下拉框对象
        /// </summary>
        /// <param name="prodId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<AttributeObjectView> GetInvAttributeByProduct(Guid prodId)
        {
            var  list = attributeBLL.GetInvAttributeByProduct(prodId);
            return list;
        }

        /// <summary>
        /// 通过产品ID获取产品下的非库存属性下拉框对象
        /// </summary>
        /// <param name="prodId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<AttributeObjectView> GetNonInvAttributeByProduct(Guid prodId)
        {
            List<AttributeObjectView> list = attributeBLL.GetNonInvAttributeByProduct(prodId);
          
            return list;
        }
    }
}
