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
    public class MerchantController : BaseApiController
    {
        IMerchantBLL merchantBLL;
        ISettingBLL settingBLL;

        public MerchantController(IComponentContext services) : base(services)
        {
            merchantBLL = Services.Resolve<IMerchantBLL>();
            settingBLL = services.Resolve<ISettingBLL>();           
        }

        
        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule)]
        public PageData<MerchantView> SearchMercLst([FromBody] MerchantPageInfo pageInfo)
        {
            PageData<MerchantView> merchVwLst = merchantBLL.GetMerchLstByCond(pageInfo);
            return merchVwLst;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule)]
        public async Task<MerchantView> GetMerchInfo(string merchID)
        {
            MerchantView merchVw = new MerchantView();
            if (!merchID.IsEmpty())
            {
                Guid mId = new Guid(merchID);
                merchVw = merchantBLL.GetMerchById(mId);              
            }
            return merchVw;
        }

        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule)]
        public async Task<SystemResult> SaveMerchInfoOnly([FromForm]MerchantView merchVw)
        {
            merchVw.Validate();
            var  result = await merchantBLL.Save(merchVw);          
            return result;
        }

        /// <summary>
        /// 啟用商家用戶
        /// </summary>
        /// <param name="merchID">商家ID</param>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule)]
        public async Task<SystemResult> ActiveMerchant(Guid merchID)
        {
            var result =  await merchantBLL.ActiveMerchantAsync(merchID);
            return result;
        }

        /// <summary>
        /// 停用商家用戶
        /// </summary>
        /// <param name="merchID">商家ID</param>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule)]
        public async Task<SystemResult> DeactiveMerchant(Guid merchID)
        {
            var result = await merchantBLL.DeactiveMerchantAsync(merchID);
            return result;
        }

        /// <summary>
        /// 删除商家
        /// </summary>
        /// <param name="recIDList"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule)]
        public async Task<SystemResult> LogicalDelMerchRec(string recIDList)
        {
            var result = await merchantBLL.LogicalDelMerchRec(recIDList);
            return result;
        }

        /// <summary>
        /// 获取系统的ShipMethod
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public SystemResult GetAdminShipMethod()
        {
            SystemResult result = new SystemResult();
            try
            {
                result.ReturnValue = merchantBLL.GetAdminShipMethod();
                result.Succeeded = true;
            }
            catch (Exception ex)
            {

                result.Succeeded = false;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 獲取商家的付運方法
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Delivery_Method })]
        public SystemResult GetShipMethodMapping(Guid merchantId)
        {
            SystemResult result = new SystemResult();
            result.ReturnValue = merchantBLL.GetMerchantShipMethods(merchantId);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 保存商家配對的付運方法
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Delivery_Method })]
        public SystemResult SaveShipMethodMapping([FromForm]MerchantShipMethodMappingView mapping)
        {
            SystemResult result = new SystemResult();
            merchantBLL.SaveShipMethodMapping(mapping);
            result.Succeeded = true;
            return result;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Promt })]
        public MerchantPromotionView GetMerchPromotionInfo(Guid merchID)
        {
            MerchantPromotionView promotion  = merchantBLL.GetMerchPromotionInfo(merchID);     
            if (promotion ==null) promotion = GetEmptyMerchPromotionInfo();
            return promotion;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Promt })]
        public MerchantPromotionView GetEmptyMerchPromotionInfo()
        {
            var langs = settingBLL.GetSupportLanguages();
            MerchantPromotionView promotion = new MerchantPromotionView();
            List<MutiLanguage> multiLanguages = new List<MutiLanguage>();
            List<MutiLanguage> covers = new List<MutiLanguage>();
            List<MutiLanguage> smallLogos = new List<MutiLanguage>();
            List<MutiLanguage> bigLogos = new List<MutiLanguage>();
            List<MutiLanguage> expdays = new List<MutiLanguage>();
            List<MutiLanguage> cooldowndays = new List<MutiLanguage>();
            promotion.CoverId = Guid.Empty;
            promotion.LogoId = Guid.Empty;
            promotion.BigLogoId = Guid.Empty;
            promotion.TAndCTranId = Guid.Empty;
            promotion.NoticeTranId = Guid.Empty;
            promotion.ReturnTermsTranId = Guid.Empty;
            promotion.OrderTranId = Guid.Empty;

            foreach (var item in langs)
            {
                multiLanguages.Add(new MutiLanguage { Desc = "", Lang = new SystemLang { Code = item.Code, Text = item.Text } });
                covers.Add(new MutiLanguage { Desc = "", Lang = new SystemLang { Code = item.Code, Text = item.Text } });
                smallLogos.Add(new MutiLanguage { Desc = "", Lang = new SystemLang { Code = item.Code, Text = item.Text } });
                bigLogos.Add(new MutiLanguage { Desc = "", Lang = new SystemLang { Code = item.Code, Text = item.Text } });
                expdays.Add(new MutiLanguage { Desc = "", Lang = new SystemLang { Code = item.Code, Text = item.Text } });
                cooldowndays.Add(new MutiLanguage { Desc = "", Lang = new SystemLang { Code = item.Code, Text = item.Text } });
            }

            promotion.Covers = covers;
            promotion.Logos = smallLogos;
            promotion.BigLogos = bigLogos;
            promotion.ExpCompleteDays = expdays;

            promotion.PromotionIntroductions = multiLanguages;

            promotion.Descriptions = multiLanguages;
            promotion.PromotionNames = multiLanguages;
            promotion.TAndCs = multiLanguages;
            promotion.Notices = multiLanguages;
            promotion.ReturnTermses = multiLanguages;
            promotion.ProductList = new List<MerchantPromotionProductView>();
            promotion.Banners = new List<MerchantPromotionBannerView>();
            promotion.ApproveStatus = ApproveType.Editing;
            promotion.LocalCoolDownDay = 7;
            promotion.OverSeaCoolDownDay = 7;
            return promotion;

        }


        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule, Function = new string[] { FunctionConst.Merch_Promt })]
        public SystemResult SaveMerchantPromotion([FromForm]MerchantPromotionView promotion)
        {
            SystemResult result = new SystemResult();
            result.Succeeded = merchantBLL.SaveMerchantPromotion(promotion);          
            return result;
        }

        [HttpGet]
        public SystemResult ApplyApprove(Guid id)
        {
            SystemResult result = merchantBLL.ApplyApprove(id);
            return result;
        }

        /// <summary>
        /// 审批通过
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public async Task<SystemResult> ApproveMerchant(string ids)
        {
            SystemResult sysRslt = new SystemResult();
            List<string> idList = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            sysRslt = await merchantBLL.ApproveMerchantAsync(idList);
            return sysRslt;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public async Task<SystemResult> RejectMerchant(Guid merchId, string reason)
        {
            SystemResult sysRslt = await merchantBLL.RejectMerchant(merchId, reason);
            return sysRslt;
        }
    }
}
