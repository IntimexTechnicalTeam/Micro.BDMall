using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDMall.Model;
using Web.Framework;
using BDMall.Domain;

namespace BDMall.BLL
{
    public interface IDeliveryBLL : IDependency
    {

        /// <summary>
        /// 获取系统语言
        /// </summary>
        /// <returns></returns>
        List<SystemLang> GetLangs();
        /// <summary>
        /// 根据国家名字，語言搜索國家
        /// </summary>
        /// <param name="name">国家名稱</param>
        /// <returns></returns>
        List<CountryDto> GetCountry(string name);
        /// <summary>
        /// 根據國家刪除國家信息
        /// </summary>
        /// <param name="ids">國家id</param>
        SystemResult DeleteCountry(List<string> ids);
        /// <summary>
        /// 根據國家獲取相關省份
        /// </summary>
        /// <param name="countryId">國家id</param>
        /// <returns></returns>
        List<ProvinceDto> GetProvinceByCountry(int countryId);

        List<KeyValue> GetProvinceByCountryZoneForSelect(int countryId, Guid zoneid, Guid id);
        /// <summary>
        /// 獲取國家信息
        /// </summary>
        /// <param name="id">國家id</param>
        /// <returns></returns>
        CountryDto GetCountryItem(int id);

        /// <summary>
        /// 獲取省份信息
        /// </summary>
        /// <param name="id">省份id</param>
        /// <returns></returns>
        ProvinceDto GetProvinceItem(int id);

        /// <summary>
        /// 批量刪除省份（州）
        /// </summary>
        /// <param name="ids">省份id</param>
        SystemResult DeleteProvince(List<string> ids);

        /// <summary>
        /// 保存省份(州)資料
        /// </summary>
        /// <param name="model">省份信息</param>
        SystemResult SaveProvince(ProvinceDto model);

        /// <summary>
        /// 保存國家信息
        /// </summary>
        /// <param name = "model" > 國家信息 </ param >
        SystemResult SaveCountry(CountryDto model);

        /// <summary>
        /// 獲取快遞公司
        /// </summary>
        /// <returns></returns>
        List<ExpressCompanyDto> GetExpress();

        /// <summary>
        /// 根据商家Id获取对应的运输方式
        /// </summary>
        /// <param name="merchId"></param>
        /// <returns></returns>
        List<ExpressCompanyDto> GetMerchantExpress(Guid merchId);


        /// <summary>
        /// 獲取快遞詳細信息
        /// </summary>
        /// <param name="id">快遞公司id</param>
        /// <returns></returns>
        ExpressCompanyDto GetExpressItem(Guid id);

        /// <summary>
        /// 根據快遞公司編號獲取詳細信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ExpressCompanyDto GetExpressItem(string code);

        /// <summary>
        /// 獲取國家列表
        /// </summary>
        /// <returns></returns>
        List<KeyValue> GetCountryForSelect();

        /// <summary>
        /// 批量刪除快遞公司
        /// </summary>
        /// <param name="ids">快遞公司id</param>
        SystemResult DeleteExpress(List<Guid> ids);

        /// <summary>
        /// 保存快遞公司
        /// </summary>
        /// <param name="model">快遞公司信息</param>
        SystemResult SaveExpress(ExpressCompanyDto model);

        /// <summary>
        /// 獲取快遞公司
        /// </summary>
        /// <returns></returns>
        List<KeyValue> GetExpressForSelect();
        /// <summary>
        /// 获取商家的运输方式
        /// </summary>
        /// <returns></returns>
        List<KeyValue> GetMerchantExpress();

        List<KeyValue> GetMerchantExpressByCond(Guid merchId);
        /// <summary>
        /// 獲取快遞適用國家
        /// </summary>
        /// <param name="id">快遞Id</param>
        /// <returns></returns>
        List<KeyValue> GetCountryByExpress(Guid id);

        /// <summary>
        /// 根據快遞獲取區域信息
        /// </summary>
        /// <param name="id">快遞id</param>
        /// <returns></returns>
        List<ExpressZoneDto> GetZone(Guid id);

        /// <summary>
        /// 獲取區域詳細信息
        /// </summary>
        /// <param name="id">區域id</param>
        /// <returns></returns>
        ZoneInfo GetZoneItem(Guid id);

        /// <summary>
        /// 保存區域信息
        /// </summary>
        /// <param name="zoneInfo">區域詳細信息</param>
        SystemResult SaveZone(ZoneInfo zoneInfo);

        /// <summary>
        /// 獲取快遞費用規則詳細信息
        /// </summary>
        /// <param name="id">費用規則id</param>
        /// <returns></returns>
        ExpressRule GetRuleItem(Guid id);

        /// <summary>
        /// 獲取快遞折扣規則詳細信息
        /// </summary>
        /// <param name="id">折扣規則id</param>
        /// <returns></returns>
        ExpressDiscount GetDiscountItem(Guid id);

        /// <summary>
        /// 獲取快遞費用規則
        /// </summary>
        /// <param name="id">快遞id</param>
        /// <returns></returns>
        List<ExpressRule> GetExpressRule(Guid id, Guid merchId);

        /// <summary>
        /// 獲取快遞折扣規則
        /// </summary>
        /// <param name="id">快遞id</param>
        /// <returns></returns>
        List<ExpressDiscount> GetDiscount(Guid id, Guid merchId);

        /// <summary>
        /// 保存快遞折扣規則
        /// </summary>
        /// <param name="model">規則信息</param>
        SystemResult SaveDiscount(ExpressDiscount model);

        /// <summary>
        /// 批量刪除快遞費用規則
        /// </summary>
        /// <param name="ids">費用規則id</param>
        SystemResult DeleteExpressRule(List<Guid> ids);

        /// <summary>
        /// 批量刪除快遞折扣規則
        /// </summary>
        /// <param name="ids">折扣規則id</param>
        SystemResult DeleteDiscount(List<Guid> ids);

        /// <summary>
        /// 根據快遞獲取區域名稱及編號
        /// </summary>
        /// <param name="id">快遞id</param>
        /// <returns></returns>
        List<KeyValue> GetZoneForSelect(Guid id);

        /// <summary>
        /// 獲取商家名稱
        /// </summary>
        /// <param name="id">商家id</param>
        /// <returns></returns>
        KeyValue GetMerchantNameBySelect(Guid id);

        /// <summary>
        /// 獲取快遞不同區域重量的詳細收費規則
        /// </summary>
        /// <param name="id">規則id</param>
        /// <returns></returns>
        List<TransPriceInfo> GetExpressPrice(Guid id);

        /// <summary>
        /// 根據快遞獲取目前規則最大重量
        /// </summary>
        /// <param name="exId">快遞編號</param>
        /// <returns></returns>
        decimal GetMaxWeightByExpress(Guid exId, Guid merchId);

        /// <summary>
        /// 保存快發費用詳細規則
        /// </summary>
        /// <param name="obj">詳細規則信息</param>
        SystemResult SaveExpressRulePrice(TransRulePrice obj);

        /// <summary>
        /// 根據國家獲取相關省份（下拉）
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        List<KeyValue> GetProvinceByCountryForSelect(int countryId);
        /// <summary>
        /// 排除已選獲取國家
        /// </summary>
        /// <param name="id"></param>
        /// <param name="zoneid"></param>
        /// <returns></returns>
        List<KeyValue> GetCountryByExpressZone(Guid id, Guid zoneid);
        /// <summary>
        /// 刪除區域
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        SystemResult DeleteZone(List<Guid> ids);
        /// <summary>
        /// 根據運費條件獲取快遞公司以及費用
        /// </summary>
        /// <param name="exCond">運費條件</param>
        /// <returns></returns>
        //List<ExpressChargeInfo> GetExpressCharge(ExpressCondition exCond);
        SystemResult GetExpressCharge(ExpressCondition exCond);
        

        /// <summary>
        /// 判斷付運選擇的國家是否存在產品中的拒送國家中
        /// </summary>
        /// <param name="vailInfo"></param>
        /// <returns></returns>
        List<ProductRefuseResult> CheckCountryIsVaild(CountryVaildInfo vailInfo);

        /// <summary>
        /// 获取快递类型是否可用
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        DeliveryTypeActiveView GetDeliveryActiveInfo(Guid merchantId);

    }
}
