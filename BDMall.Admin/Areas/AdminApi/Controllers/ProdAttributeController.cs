using Autofac;
using BDMall.BLL;
using Intimex.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
    }
}
