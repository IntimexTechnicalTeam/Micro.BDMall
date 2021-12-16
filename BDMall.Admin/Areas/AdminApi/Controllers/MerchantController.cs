using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
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
    public class MerchantController : BaseApiController
    {
        IMerchantBLL merchantBLL;

        public MerchantController(IComponentContext services) : base(services)
        {
            merchantBLL = Services.Resolve<IMerchantBLL>();
        }

        /// <summary>
        /// 獲取商家列表的下拉框資源
        /// </summary>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule)]
        public List<KeyValue> GetMerchantOptions()
        {
            List<KeyValue> keyValLIst = merchantBLL.GetMerchantCboSrcByCond(true);
            return keyValLIst;
        }
    }
}
