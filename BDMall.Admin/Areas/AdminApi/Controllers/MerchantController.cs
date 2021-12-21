using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    public class MerchantController : BaseApiController
    {
        IMerchantBLL merchantBLL;
        ISettingBLL settingBLL;

        public MerchantController(IComponentContext services) : base(services)
        {
            merchantBLL = Services.Resolve<IMerchantBLL>();
            settingBLL = services.Resolve<ISettingBLL>();
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

        [HttpGet]
        //[AdminApiAuthorize(Module = ModuleConst.MerchantModule)]
        public List<KeyValue> GetApproveStatusList()
        {
            var statusList = new List<KeyValue>();
            statusList = settingBLL.GetApproveStatuses();       
            return statusList;
        }

        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule)]
        public PageData<MerchantView> SearchMercLst([FromForm] MerchantPageInfo pageInfo)
        {
            PageData<MerchantView> merchVwLst = new PageData<MerchantView>();

            merchVwLst = merchantBLL.GetMerchLstByCond(pageInfo);

            return merchVwLst;
        }
    }
}
