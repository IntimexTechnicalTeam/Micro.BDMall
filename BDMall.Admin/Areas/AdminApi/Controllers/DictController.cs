using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

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
        public IAttributeBLL attributeBLL;
        public ICountryBLL countryBLL;
        
        public DictController(IComponentContext services) : base(services)
        {
            codeMasterBLL = Services.Resolve<ICodeMasterBLL>();
            settingBLL = Services.Resolve<ISettingBLL>();
            merchantBLL = Services.Resolve<IMerchantBLL>();
            attributeBLL = Services.Resolve<IAttributeBLL>();
            countryBLL = Services.Resolve<ICountryBLL>();
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

        [HttpGet]
        public List<KeyValue> GetCountry()
        {         
            var countrys = countryBLL.GetCountry();
            return countrys;
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
                list = langs.Select(item => new KeyValue { Id = item.Id.ToString(), Text = item.Text }).ToList();
            }
            catch (BLException blex)
            {

            }

            return list;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule)]
        public List<KeyValue> GetMerchantOptionsNoMall()
        {
            List<KeyValue> keyValLIst = merchantBLL.GetMerchantCboSrcByCond(false);
            return keyValLIst;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.MerchantModule)]
        public List<KeyValue> GetMerchantCboSrc()
        {
            List<KeyValue> keyValLIst = merchantBLL.GetMerchantCboSrcByCond(true);
            return keyValLIst;
        }

        /// <summary>
        /// 獲取屬性的佈局
        /// </summary>
        public List<KeyValue> GetAttrLayout()
        {
            var list = attributeBLL.GetAttrLayout();
            return list;
        }

        [HttpGet]
        public List<KeyValue> GetCMCalculateTypes()
        {
            List<KeyValue> typeList = settingBLL.GetCMCalculateTypes();
            return typeList;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule)]
        public List<KeyValue> GetProductUnit()
        {
            List<KeyValue> list = new List<KeyValue>();
            var masters = codeMasterBLL.GetCodeMasters(CodeMasterModule.Setting, CodeMasterFunction.ProductUnit);
            list = masters.Select(s => new KeyValue { Id = s.Value, Text = s.Description }).ToList();
            return list;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule)]
        public List<KeyValue> GetCTNUnit()
        {
            List<KeyValue> list = new List<KeyValue>();
            var masters = codeMasterBLL.GetCodeMasters(CodeMasterModule.Setting, CodeMasterFunction.CTNUnit);
            list = masters.Select(s => new KeyValue { Id = s.Value, Text = s.Description }).ToList();
            return list;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule)]
        public List<KeyValue> GetWeightUnit()
        {
            List<KeyValue> list = new List<KeyValue>();
            var masters = codeMasterBLL.GetCodeMasters(CodeMasterModule.Setting, CodeMasterFunction.WeightUnit);
            list = masters.Select(s => new KeyValue { Id = s.Value, Text = s.Description }).ToList();
            return list;
        }

    }
}
