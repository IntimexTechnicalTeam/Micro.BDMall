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
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using BDMall.Model;

namespace BDMall.Admin.Areas.AdminAPI.Controllers
{

    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    [AdminApiAuthorize(Module = ModuleConst.DeliveryModule)]
    public class DeliveryController : BaseApiController
    {
        IDeliveryBLL DeliveryBLL;

        public DeliveryController(IComponentContext services) : base(services)
        {
            DeliveryBLL = Services.Resolve<IDeliveryBLL>();
        }

        /// <summary>
        /// 獲取所有Country信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public List<CountryDto> GetCountry(string name)
        {
            List<CountryDto> list = new List<CountryDto>();

            list = DeliveryBLL.GetCountry(name);

            return list;
        }
        /// <summary>
        /// 獲取所有ExpressCountry
        /// </summary>
        /// <param name = "id" ></ param >
        /// < returns ></returns >
        [HttpGet]
        [AllowAnonymous]
        [AdminApiAuthorize(Module = ModuleConst.PersonalSetting)]
        public List<KeyValue> GetCountryForExpress()
        {
            var lists = DeliveryBLL.GetCountryForSelect().ToList();

            return lists;
        }
        /// <summary>
        /// 獲取快遞適用國家
        /// </summary>
        /// <param name = "id" >快遞id</param >
        /// <returns></returns >
        [HttpGet]
        [AllowAnonymous]
        public List<KeyValue> GetExpressCountry(Guid id)
        {
            var list = DeliveryBLL.GetCountryByExpress(id);
            return list;
        }
        /// <summary>
        /// 獲取快遞適用國家
        /// </summary>
        /// <param name = "id" >快遞id</param >
        /// <param name = "zoneid" >區域id</param >
        /// <returns></returns >
        [HttpGet]
        [AllowAnonymous]
        public List<KeyValue> GetExpressCountryZone(Guid id, Guid zoneid)
        {
            var list = DeliveryBLL.GetCountryByExpressZone(id, zoneid);
            return list;
        }
        /// <summary>
        /// 通過id刪除国家
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        [HttpGet]
        public SystemResult DeleteCountry(string skus)
        {
            string[] ids = skus.Split(',');
            SystemResult result = DeliveryBLL.DeleteCountry(ids.ToList());
            return result;
        }
        /// <summary>
        /// 獲取屬性下的值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]

        public CountryDto GetCountryItem(int id)
        {
            return DeliveryBLL.GetCountryItem(id);

        }
        /// <summary>
        /// 保存Country
        /// </summary>
        /// <param name ="country" ></param >
        /// <returns></returns >
        [HttpPost]
        public SystemResult SaveCountry(CountryDto country)
        {
            SystemResult result = DeliveryBLL.SaveCountry(country);
            return result;
        }
        /// <summary>
        /// 獲取Province信息
        /// </summary>
        /// /// <param name="countryid">國家編號</param>
        /// <returns></returns>
        [HttpGet]

        public List<ProvinceDto> GetProvince(int countryid)
        {
            return DeliveryBLL.GetProvinceByCountry(countryid);

        }
        /// <summary>
        /// 獲取屬性下的值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]

        public ProvinceDto GetProvinceItem(int id)
        {

            ProvinceDto attr = DeliveryBLL.GetProvinceItem(id);

            return attr;
        }
        /// <summary>
        /// 保存Province
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        [HttpPost]

        public SystemResult SaveProvince(ProvinceDto province)
        {
            SystemResult result = DeliveryBLL.SaveProvince(province);
            return result;
        }
        /// <summary>
        /// 通過id刪除国家
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public SystemResult DeleteProvince(string skus)
        {
            string[] ids = skus.Split(',');
            SystemResult result = DeliveryBLL.DeleteProvince(ids.ToList());
            return result;
        }
        /// <summary>
        /// 獲取快遞信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ExpressCompanyDto> GetExpress()
        {
            List<ExpressCompanyDto> list = DeliveryBLL.GetExpress();
            return list;
        }
        /// <summary>
        /// 獲取屬性下的值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpGet]

        public ExpressCompanyDto GetExpressItem(Guid id)
        {

            ExpressCompanyDto item = DeliveryBLL.GetExpressItem(id);
            return item;
        }
        /// <summary>
        /// 通過id刪除国家
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public SystemResult ExpressDelete(string skus)
        {
            string[] ids = skus.Split(',');
            SystemResult result = DeliveryBLL.DeleteExpress(ids.Select(d => new Guid(d)).ToList());
            return result;
        }
        /// <summary>
        /// 保存Province
        /// </summary>
        /// <param name = "exInfo"></param>
        /// <returns ></returns >
        [HttpPost]
        public SystemResult SaveExpress([FromForm] ExpressCompanyDto exInfo)
        {
            SystemResult result = DeliveryBLL.SaveExpress(exInfo);
            return result;
        }
        /// <summary>
        /// 獲取Province信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public List<KeyValue> GetExpressCompany()
        {
            List<KeyValue> list = DeliveryBLL.GetExpressForSelect();
            return list;
        }

        [HttpGet]
        public List<KeyValue> GetMerchantExpress()
        {
            List<KeyValue> list = DeliveryBLL.GetMerchantExpress();
            return list;
        }

        [HttpGet]
        public List<KeyValue> GetMerchantExpressByCond(Guid id)
        {
            List<KeyValue> list = DeliveryBLL.GetMerchantExpressByCond(id);
            return list;
        }

        /// <summary>
        /// 獲取Province信息
        /// </summary>
        /// /// <param name="exId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public List<ExpressZoneDto> GetZone(Guid exId)
        {
            List<ExpressZoneDto> list = DeliveryBLL.GetZone(exId);

            return list;
        }
        /// <summary>
        /// 獲取屬性下的值
        /// </summary>
        /// <param name = "id" ></param >
        /// <returns></returns >
        [HttpGet]
        public ZoneInfo GetZoneItem(Guid id)
        {
            ZoneInfo attr = DeliveryBLL.GetZoneItem(id);
            return attr;
        }

        /// <summary>
        /// 通過國家id讀取省份
        /// </summary>
        /// <param name = "countryId"></param>
        /// <returns ></returns >
        [HttpGet]

        public List<KeyValue> GetProvinceByCountry(int countryId)
        {
            List<KeyValue> list = DeliveryBLL.GetProvinceByCountryForSelect(countryId);
            return list;
        }
        /// <summary>
        /// 通過國家id讀取省份
        /// </summary>
        /// <param name = "countryId"></param>
        /// /// <param name = "zoneId"></param>
        /// <returns ></returns >
        [HttpGet]

        public List<KeyValue> GetProvinceByCountryZoneForSelect(int countryId, Guid zoneId, Guid exId)
        {
            List<KeyValue> list = DeliveryBLL.GetProvinceByCountryZoneForSelect(countryId, zoneId, exId);
            return list;
        }
        /// <summary>
        /// 通過id刪除国家
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public SystemResult ZoneDelete(string skus)
        {
            string[] ids = skus.Split(',');
            SystemResult result = DeliveryBLL.DeleteZone(ids.Select(d => new Guid(d)).ToList());
            return result;
        }
        /// <summary>
        /// 獲取Province信息
        /// </summary>
        /// /// <param name="exId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public List<ExpressRule> GetTransRule(Guid exId, Guid merchId)
        {
            List<ExpressRule> list = DeliveryBLL.GetExpressRule(exId, merchId);
            return list;
        }
        /// <summary>
        /// 獲取Province信息
        /// </summary>
        /// /// <param name="exId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public List<ExpressDiscount> GetDiscount(Guid exId, Guid merchId)
        {
            List<ExpressDiscount> list = DeliveryBLL.GetDiscount(exId, merchId);

            return list;
        }
        /// <summary>
        /// 通過id刪除国家
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public SystemResult DiscountDelete(string skus)
        {
            string[] ids = skus.Split(',');
            SystemResult result = DeliveryBLL.DeleteDiscount(ids.Select(d => new Guid(d)).ToList());
            return result;
        }
        /// <summary>
        /// 通過id刪除国家
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public SystemResult DeleteTransRule(string skus)
        {
            string[] ids = skus.Split(',');
            SystemResult result = DeliveryBLL.DeleteExpressRule(ids.Select(d => new Guid(d)).ToList());
            return result;
        }
        /// <summary>
        /// 獲取Province信息
        /// </summary>
        /// /// <param name="ruleId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public List<TransPriceInfo> GetTransPrice(Guid ruleId)
        {
            List<TransPriceInfo> list = DeliveryBLL.GetExpressPrice(ruleId);
            return list;
        }
        /// <summary>
        /// 獲取屬性下的值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]

        public ExpressDiscount GetDiscountItem(Guid id)
        {
            ExpressDiscount attr = DeliveryBLL.GetDiscountItem(id);
            return attr;
        }
        /// <summary>
        /// 保存Discount
        /// </summary>
        /// <param name="discount"></param>
        /// <returns></returns>
        [HttpPost]

        public SystemResult SaveDiscount(ExpressDiscount discount)
        {
            SystemResult result = DeliveryBLL.SaveDiscount(discount);
            return result;
        }
        /// <summary>
        /// 獲取屬性下的值
        /// </summary>
        /// <param name = "id" ></param>
        /// <returns ></returns >
        [HttpGet]

        public ExpressRule GetRuleItem(Guid id)
        {
            ExpressRule attr = DeliveryBLL.GetRuleItem(id);
            return attr;
        }

        /// <summary>
        /// 保存Province
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]

        public SystemResult SaveTransRulePrice([FromForm] TransRulePrice obj)
        {
            SystemResult result = DeliveryBLL.SaveExpressRulePrice(obj);
            return result;
        }
        /// <summary>
        /// 獲取所有ExpressCountry信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public List<KeyValue> GetZoneT(Guid id)
        {
            var list = DeliveryBLL.GetZoneForSelect(id).ToList();
            return list;
        }

        /// <summary>
        /// 獲取商家名稱
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public KeyValue GetMerchantName(Guid id)
        {
            var list = DeliveryBLL.GetMerchantNameBySelect(id);
            return list;

        }
        /// <summary>
        /// 保存Province
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]

        public SystemResult SaveZone(ZoneInfo obj)
        {
            SystemResult result = DeliveryBLL.SaveZone(obj);
            return result;
        }

        /// <summary>
        /// 根据快递获取收费规则的目前最大重量
        /// </summary>
        /// <param name="exId">快递编号</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public decimal GetMaxWeightByExpress(Guid exId, Guid merchId)
        {
            var list = DeliveryBLL.GetMaxWeightByExpress(exId, merchId);
            return list;
        }
        /// <summary>
        /// 获取系统所支持语言
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public List<SystemLang> GetLangs()
        {
            List<SystemLang> list = new List<SystemLang>();
            list = DeliveryBLL.GetLangs();

            return list;
        }

    }
}
