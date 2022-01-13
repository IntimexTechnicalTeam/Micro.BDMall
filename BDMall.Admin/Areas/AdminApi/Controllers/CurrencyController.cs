using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using Intimex.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    /// <summary>
    /// 計劃任務
    /// </summary>
    public class CurrencyController : BaseApiController
    {

        ICurrencyBLL CurrencyBLL;
        public CurrencyController(IComponentContext services) : base(services)
        {
            CurrencyBLL = services.Resolve<ICurrencyBLL>();
        }
        /// <summary>
        /// 獲取Currency信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<CurrencyExchangeRateDto> GetCurrItems(string baseCode)
        {
            //SystemResult result = new SystemResult();
            List<CurrencyExchangeRateDto> list = null;
            try
            {
                list = CurrencyBLL.GetCurrExchangeRate(baseCode);
            }
            catch (BLException blex)
            {
                //result.Succeeded = false;
            }
            return list;
        }

        ///// <summary>
        ///// 獲取CurrencyCode表的詳細信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public CurrencyDto GetCurrDetail(Guid id)
        //{
        //    CurrencyDto currency = CurrencyBLL.GetCurrency(id);
        //    return currency;

        //}

        /// <summary>
        /// 獲取基准货币
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBaseCurrency()
        {

            return CurrencyBLL.GetDefaultCurrencyCode();
        }


        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public SystemResult SetDefault(string code)
        {
            SystemResult result = new SystemResult();

            CurrencyBLL.SetDefault(code);
            result.Succeeded = true;
            result.Message = "";
            return result;
        }

        /// <summary>
        /// 更新Currency對應匯率
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public SystemResult UpdateRate([FromForm] CurrencyListView data)
        {
            SystemResult result = new SystemResult();

            CurrencyBLL.UpdateRate(data);
            result.Succeeded = true;
            result.Message = "";
            return result;

        }

        /// <summary>
        /// 獲取CurrencyCode下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValue> GetCurrList()
        {
            //SystemResult result = new SystemResult();
            List<KeyValue> list = new List<KeyValue>();
            list = CurrencyBLL.GetCurrList();
            return list;
        }

        [HttpGet]
        public CurrencyView GetCurrencyByCode(string code)
        {
            var currencyVw = new CurrencyView();
            currencyVw = CurrencyBLL.GetCurrencyByCode(code);
            return currencyVw;
        }

        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public List<CurrencyView> SearchCurrencyList([FromBody] CurrencyPageInfo pageInfo)
        {
            var currencyList = new List<CurrencyView>();
            currencyList = CurrencyBLL.SearchCurrencyList(pageInfo);
            return currencyList;
        }

        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public SystemResult SaveCurrency([FromForm] CurrencyView currencyvW)
        {
            var sysRslt = new SystemResult();
            if (currencyvW != null)
            {
                if (currencyvW.IsModify)
                {
                    return CurrencyBLL.UpdateCurrency(currencyvW);
                }
                else
                {
                    return CurrencyBLL.InsertCurrency(currencyvW);
                }
            }

            return sysRslt;
        }

        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public SystemResult DeleteCurrency([FromBody] CurrencyView currencyvW)
        {
            var sysRslt = new SystemResult();
            return CurrencyBLL.DeleteCurrency(currencyvW);
            return sysRslt;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
        public SystemResult DeleteCurrencyList(string recIdList)
        {
            var sysRslt = new SystemResult();
            sysRslt = CurrencyBLL.DeleteCurrencyList(recIdList);
            return sysRslt;
        }
    }

}
