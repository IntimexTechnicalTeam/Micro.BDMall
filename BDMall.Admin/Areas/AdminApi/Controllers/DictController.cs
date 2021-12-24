using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using Intimex.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Intimex.Common;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using BDMall.Domain;
using BDMall.Enums;
using System.Threading;
using System.Globalization;
using BDMall.BLL;

namespace BDMall.Admin.Areas.AdminAPI.Controllers
{

    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    [AdminApiAuthorize(Module = ModuleConst.PersonalSetting)]
    public class DictController : BaseApiController
    {
        public ICodeMasterBLL codeMasterBLL;
        public ISettingBLL settingBLL;
        public IMerchantBLL merchantBLL;

        public DictController(IComponentContext services) : base(services)
        {
            codeMasterBLL = Services.Resolve<ICodeMasterBLL>();
            settingBLL = Services.Resolve<ISettingBLL>();
            merchantBLL = Services.Resolve<IMerchantBLL>();   
        }

        /// <summary>
        /// 獲取郵件類型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<KeyValue>> GetEmailType()
        {
            return new List<KeyValue>();
        }

        /// <summary>
        /// 獲取推廣郵件中的圖片尺寸
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<KeyValue>> GetPromoteImgSize()
        {
            return new List<KeyValue>();
        }

        [HttpGet]
        public async Task<List<KeyValue>> GetWhether()
        {
            string userLang = CurrentUser?.Lang.ToString() ?? "E";
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CultureHelper.GetSupportCulture(userLang));
            Resources.Value.Culture = Thread.CurrentThread.CurrentCulture;

            List<KeyValue> list = new List<KeyValue>();

            list.Add(new KeyValue { Id = Resources.Label.Yes, Text = Resources.Label.Yes });
            list.Add(new KeyValue { Id = Resources.Label.No, Text = Resources.Label.No });
            return list;
        }


        /// <summary>
        /// 獲取字碼主檔Module信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<KeyValue>> GetCodeMasterModules()
        {

            return new List<KeyValue>();
        }

        /// <summary>
        /// 獲取批量更新產品價格類型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValue> GetUpdPriceType()
        {
            //var lang = WSCookie.GetUserLanguage();
            //Thread.CurrentThread.CurrentCulture = new CultureInfo(CultureHelper.GetSupportCulture(lang));
            //Resources.Value.Culture = Thread.CurrentThread.CurrentCulture;

            List<KeyValue> list = new List<KeyValue>();

            //list.Add(new KeyValue { Id = ((int)UpdPriceTypeEnum.Increase).ToString(), Text = Resources.Value.Increase });
            //list.Add(new KeyValue { Id = ((int)UpdPriceTypeEnum.Decrease).ToString(), Text = Resources.Value.Decrease });
            return list;
        }
        /// <summary>
        /// 獲取字碼主檔Funcionts信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<KeyValue>> GetCodeMasterFunction()
        {
            //List<KeyValue> list = new List<KeyValue>();


            //foreach (CodeMasterFunction codeMstrFunc in Enum.GetValues(typeof(CodeMasterFunction)))
            //{
            //    list.Add(new KeyValue { Id = codeMstrFunc.ToString(), Text = codeMstrFunc.ToString() });
            //}


            //return list;

              return new List<KeyValue>();
        }

        /// <summary>
        /// 獲取圖片正反面
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValue>> GetImageSide()
        {
            //List<KeyValue> list = new List<KeyValue>();

            //list = DictBLL.GetImageSide();

            //return list;

            return new List<KeyValue>();
        }

        /// <summary>
        /// 返回可支持語言的
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<MutiLanguage>> GetUseLanguage()
            {

            return new List<MutiLanguage>();
        }

     
        /// <summary>
        /// 送貨方式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<KeyValue>> GetSendProductMethod()
        {
            return new List<KeyValue>();
        }

        /// <summary>
        /// 獲取付款方式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<KeyValue>> GetPaymentMethod()
        {
            return new List<KeyValue>();
        }
        /// <summary>
        /// 獲取訂單狀態
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<KeyValue>> GetOrderStatus()
        {
            return new List<KeyValue>();
        }

        /// <summary>
        /// 獲取SearchKeyType
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<KeyValue>> GetSearchKeyType()
        {
            return new List<KeyValue>();
        }

        /// <summary>
        /// 獲取Report Type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<KeyValue>> GetReportType()
                {
            return new List<KeyValue>();
                }

        [HttpGet]
        public async Task<List<KeyValue>> GetWhseComboSrc()
        {
            return new List<KeyValue>();
            }

        [HttpGet]
        public async Task<List<KeyValue>> GetWhseComboSrcByMerchant(Guid merchantId)
            {
            return new List<KeyValue>();
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

        /// <summary>
        /// 獲取系統支持的語言種類
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValue> GetSupportLanguage()
        {
            List<KeyValue> list = new List<KeyValue>();
            string userLang = CurrentUser.Lang.ToString();
            try
            {

                var langs = settingBLL.GetSupportLanguages();

                foreach (var item in langs)
                {
                    KeyValue entity = new KeyValue();
                    entity.Id = item.Code;
                    entity.Text = item.Text;
                    list.Add(entity);
                }
            }
            catch (BLException blex)
            {

            }

            return list;
        }
    }
}
