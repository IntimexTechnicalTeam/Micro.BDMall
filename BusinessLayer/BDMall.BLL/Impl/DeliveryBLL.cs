using BDMall.Domain;
using BDMall.ECShip;
using BDMall.ECShip.Model.Calculator;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Utility;
using Intimex.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Framework;

namespace BDMall.BLL
{
    public class DeliveryBLL : BaseBLL, IDeliveryBLL
    {
        ITranslationRepository _translationRepository;
        IProductRepository _productRepository;
        IMerchantFreeChargeRepository _merchantFreeChargeRepository;
        IMerchantShipMethodMappingRepository _merchantShipMethodMappingRepository;
        IProvinceRepository _provinceRepository;
        ICityRepository _cityRepository;
        ISettingBLL _settingBLL;
        ICodeMasterBLL _codeMasterBLL;
        IMerchantBLL _merchantBLL;
        IDeliveryAddressBLL _deliveryAddressBLL;
        private List<MutiLanguage> _EmptyLangs;
        public DeliveryBLL(IServiceProvider services) : base(services)
        {
            _translationRepository = Services.Resolve<ITranslationRepository>();
            _productRepository = Services.Resolve<IProductRepository>();
            _merchantFreeChargeRepository = Services.Resolve<IMerchantFreeChargeRepository>();
            _merchantShipMethodMappingRepository = Services.Resolve<IMerchantShipMethodMappingRepository>();
            _settingBLL = Services.Resolve<ISettingBLL>();
            _codeMasterBLL = Services.Resolve<ICodeMasterBLL>();
            _merchantBLL = Services.Resolve<IMerchantBLL>();
            _deliveryAddressBLL = Services.Resolve<IDeliveryAddressBLL>();
            _provinceRepository = Services.Resolve<IProvinceRepository>();
            _cityRepository = Services.Resolve<ICityRepository>();
        }

        public List<ProductRefuseResult> CheckCountryIsVaild(CountryVaildInfo vailInfo)
        {
            List<ProductRefuseResult> result = new List<ProductRefuseResult>();
            var productRefuseList = baseRepository.GetList<ProductRefuseDelivery>().Where(p => p.CountryId == vailInfo.CountryId).ToList();

            if (vailInfo.ProductIds != null)
            {
                foreach (var item in vailInfo.ProductIds)
                {
                    ProductRefuseResult info = new ProductRefuseResult();
                    var sku = Guid.Parse(item);
                    var refuseProduct = productRefuseList.FirstOrDefault(p => p.ProductId == sku);
                    info.Sku = item;
                    if (refuseProduct != null)
                    {
                        info.IsRefuse = true;
                    }
                    else
                    {
                        info.IsRefuse = false;
                    }
                    result.Add(info);
                }
            }


            return result;
        }

        public SystemResult DeleteCountry(List<string> ids)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;//统一提交

            var items = baseRepository.GetList<Country>().Where(d => ids.Contains(d.Id.ToString()));
            foreach (var item in items)
            {
                item.IsDeleted = true;
                baseRepository.Update(item);
            }
            UnitOfWork.Submit();
            result.Succeeded = true;

            return result;
        }

        public SystemResult DeleteDiscount(List<Guid> ids)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;//统一提交

            var items = baseRepository.GetList<ExpressDiscount>().Where(d => ids.Contains(d.Id));
            foreach (var item in items)
            {
                item.IsDeleted = true;
                baseRepository.Update(item);
            }
            UnitOfWork.Submit();
            result.Succeeded = true;

            return result;
        }

        public SystemResult DeleteExpress(List<Guid> ids)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;//统一提交

            var items = baseRepository.GetList<ExpressCompany>().Where(d => ids.Contains(d.Id));
            foreach (var item in items)
            {
                item.IsDeleted = true;
                baseRepository.Update(item);
            }
            UnitOfWork.Submit();
            result.Succeeded = true;

            return result;
        }

        public SystemResult DeleteExpressRule(List<Guid> ids)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;//统一提交

            var items = baseRepository.GetList<ExpressRule>().Where(d => ids.Contains(d.Id));
            foreach (var item in items)
            {
                item.IsDeleted = true;
                baseRepository.Update(item);
                //物理刪除由規則生成的價格

            }
            DeleteRulePrice(ids);
            UnitOfWork.Submit();
            result.Succeeded = true;

            return result;
        }
        /// <summary>
        /// 物理刪除快遞收費規則所生成的具體價格
        /// </summary>
        /// <param name="ruleId">規則編號</param>
        private void DeleteRulePrice(List<Guid> ruleId)
        {
            var old = baseRepository.GetList<ExpressPrice>().Where(d => ruleId.Contains(d.RuleId)).Select(d => d).ToList();
            if (old != null)
            {
                foreach (var id in old)
                {
                    baseRepository.Delete(id);
                }
            }
        }
        private void DeleteRulePrice(Guid ruleId)
        {
            var old = baseRepository.GetList<ExpressPrice>().Where(d => d.RuleId == ruleId).Select(d => d).ToList();
            if (old != null)
            {
                foreach (var id in old)
                {
                    baseRepository.Delete(id);
                }
            }
        }
        /// <summary>
        /// 物理刪除特定區域的收費規則
        /// </summary>
        /// <param name="zoneIds"></param>
        private void DeleteRulePriceByZone(List<Guid> zoneIds)
        {
            var old = baseRepository.GetList<ExpressPrice>().Where(d => zoneIds.Contains(d.ZoneId)).Select(d => d).ToList();
            if (old != null)
            {
                foreach (var id in old)
                {
                    baseRepository.Delete(id);
                }
            }
        }
        public SystemResult DeleteProvince(List<string> ids)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;//统一提交

            var items = baseRepository.GetList<Province>().Where(d => ids.Contains(d.Id.ToString()));
            foreach (var item in items)
            {
                item.IsDeleted = true;
                baseRepository.Update(item);
            }
            UnitOfWork.Submit();
            result.Succeeded = true;

            return result;
        }

        public SystemResult DeleteZone(List<Guid> ids)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;//统一提交

            var items = baseRepository.GetList<ExpressZone>().Where(d => ids.Contains(d.Id));
            foreach (var item in items)
            {
                item.IsDeleted = true;
                baseRepository.Update(item);
            }
            //物理刪除收費規則
            DeleteRulePriceByZone(ids);
            UnitOfWork.Submit();
            result.Succeeded = true;

            return result;
        }

        public List<CountryDto> GetCountry(string name)
        {
            List<CountryDto> list = new List<CountryDto>();

            var query = baseRepository.GetList<Country>();

            if (!name.IsEmpty())
                query = query.Where(d => d.IsDeleted == false && (d.Name.Contains(name) || d.Name_e.Contains(name) || d.Name_c.Contains(name) || d.Name_s.Contains(name) || d.Name_j.Contains(name)));

            list = AutoMapperExt.MapToList<Country, CountryDto>(query.ToList());
            return list;
        }

        public List<KeyValue> GetCountryByExpress(Guid id)
        {

            var data = (from ec in baseRepository.GetList<ExpressCountry>()
                        join c in baseRepository.GetList<Country>() on ec.CountryId equals c.Id
                        where ec.ExpressId == id && ec.IsDeleted == false
                        select c
                        ).ToList();

            var model = AutoMapperExt.MapTo<List<CountryDto>>(data);

            var list = model.Select(d => new KeyValue
            {
                Id = d.Id.ToString(),
                Text = NameUtil.GetCountryName(CurrentUser.Lang.ToString(), d),
            }).ToList();
            return list;
        }

        public List<KeyValue> GetCountryByExpressZone(Guid id, Guid zoneid)
        {
            var ids = baseRepository.GetList<ExpressCountry>().Where(d => d.ExpressId == id && d.IsDeleted == false).Select(d => d.CountryId).ToList();
            List<int> selectedCountry = new List<int>();
            var orderZone = baseRepository.GetList<ExpressZone>().Where(d => d.ExpressId == id && !d.Id.Equals(zoneid) && d.IsDeleted == false).Select(d => d.Id).ToList();
            if (orderZone?.Count() > 0)
            {
                var selectedCountryTemp = baseRepository.GetList<ExpressZoneCountry>().Where(d => orderZone.Contains(d.ZoneId)).Select(d => d.CountryId).ToList();
                foreach (var cid in selectedCountryTemp)
                {
                    var proNum = baseRepository.GetList<Province>().Where(d => d.CountryId == cid && d.IsDeleted == false).Count();
                    var selproNum = baseRepository.GetList<ExpressZoneProvince>().Where(d => d.CountryId == cid && d.ZoneId == zoneid && d.IsDeleted == false).Count();
                    if (proNum == selproNum)
                    {
                        selectedCountry.Add(cid);
                    }
                }
            }
            var data = baseRepository.GetList<Country>().Where(p => ids.Contains(p.Id) && !selectedCountry.Contains(p.Id)).ToList();
            var model = AutoMapperExt.MapTo<List<CountryDto>>(data);
            var list = model.Select(d => new KeyValue
            {
                Id = d.Id.ToString(),
                Text = NameUtil.GetCountryName(CurrentUser.Lang.ToString(), d),
            }).ToList();
            return list;
        }

        public List<KeyValue> GetCountryForSelect()
        {
            var lists = baseRepository.GetList<Country>().Where(d => d.IsDeleted == false).ToList();
            var model = AutoMapperExt.MapTo<List<CountryDto>>(lists);
            List<KeyValue> list = model.Select(d => new KeyValue
            {
                Id = d.Id.ToString(),
                Text = NameUtil.GetCountryName(CurrentUser.Lang.ToString(), d),
            }).ToList();
            return list;
        }

        public CountryDto GetCountryItem(int id)
        {
            CountryDto country = new CountryDto();
            var query = baseRepository.GetModel<Country>(p => p.Id == id);

            country = AutoMapperExt.MapTo<CountryDto>(query);
            var langs = GetSupportLanguage();
            if (country != null)
            {
                country.Names = LangUtil.GetMutiLang<CountryDto>(country, "Name", langs);
            }
            else
            {
                country = new CountryDto();
                country.Names = LangUtil.GetMutiLang<CountryDto>(country, "Name", langs);
            }
            country.Procince = GetProvinceByCountry(id);
            return country;
        }

        public DeliveryTypeActiveView GetDeliveryActiveInfo(Guid merchantId)
        {

            DeliveryTypeActiveView result = new DeliveryTypeActiveView();
            var activeShipmethod = _merchantBLL.GetMerchantShipMethods(merchantId);

            if (activeShipmethod != null)
            {
                if (activeShipmethod.MerchantShipMethods != null)
                {
                    var DRecords = activeShipmethod.MerchantShipMethods.Where(p => p.ShipMethodCode != "CC" && p.ShipMethodCode != "IPS" && p.IsEffect == true).ToList();
                    var PRecords = activeShipmethod.MerchantShipMethods.Where(p => p.ShipMethodCode == "CC" && p.IsEffect == true).ToList();
                    var ZRecords = activeShipmethod.MerchantShipMethods.Where(p => p.ShipMethodCode == "IPS" && p.IsEffect == true).ToList();

                    result.D = DRecords.Count > 0;
                    result.P = PRecords.Count > 0;
                    result.Z = ZRecords.Count > 0;
                }
            }
            return result;

        }

        public List<ExpressDiscount> GetDiscount(Guid id, Guid merchId)
        {
            List<ExpressDiscount> list = baseRepository.GetList<ExpressDiscount>().Where(d => d.ExpressId == id && d.MerchantId == merchId && d.IsDeleted == false).ToList();
            return list;

        }

        public ExpressDiscount GetDiscountItem(Guid id)
        {
            ExpressDiscount obj = new ExpressDiscount();
            var item = baseRepository.GetModel<ExpressDiscount>(p => p.Id == id);
            if (item != null)
            {
                obj = item;
            }
            return obj;
        }

        public List<ExpressCompanyDto> GetExpress()
        {
            var langs = GetSupportLanguage();
            List<ExpressCompany> list = baseRepository.GetList<ExpressCompany>().Where(d => d.IsDeleted == false).ToList();

            List<ExpressCompanyDto> datas = new List<ExpressCompanyDto>();
            datas = AutoMapperExt.MapToList<ExpressCompany, ExpressCompanyDto>(list);
            foreach (var item in datas)
            {
                item.Names = _translationRepository.GetMutiLanguage(item.NameTransId);
                item.Name = langs == null ? "" : item.Names.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc;
            }
            return datas;
        }

        public SystemResult GetExpressCharge(ExpressCondition exCond)
        {
            SystemResult result = new SystemResult();

            var data = new List<ExpressChargeInfo>();

            try
            {
                if (exCond == null)
                {
                    return null;
                }

                decimal totalWeight = exCond.TotalWeight;
                Guid deliveryAddressId = exCond.DeliveryAddrId;

                if (totalWeight == 0)
                {
                    totalWeight = 0.1M;
                }

                var mbrAddress = _deliveryAddressBLL.GetAddress(deliveryAddressId);
                if (mbrAddress != null)
                {
                    int countryId = mbrAddress.CountryId;
                    int provinceId = mbrAddress.ProvinceId;

                    var expressPriceVwLst = (from p in baseRepository.GetList<ExpressPrice>()
                                             join z in baseRepository.GetList<ExpressZone>() on p.ZoneId equals z.Id
                                             join zc in baseRepository.GetList<ExpressZoneCountry>() on z.Id equals zc.ZoneId
                                             join c in baseRepository.GetList<Country>() on zc.CountryId equals c.Id
                                             join zpv in baseRepository.GetList<ExpressZoneProvince>()
                                             on new { CountryId = c.Id, ZoneId = z.Id } equals new { zpv.CountryId, zpv.ZoneId }
                                             join r in baseRepository.GetList<ExpressRule>() on p.RuleId equals r.Id
                                             join cp in baseRepository.GetList<ExpressCompany>() on r.ExpressId equals cp.Id
                                             where c.Id == countryId
                                             && zpv.ProvinceId == provinceId
                                             && cp.Code != ExpressCompanyEnum.ECS.ToString()
                                             && p.IsActive && !p.IsDeleted
                                             && z.IsActive && !z.IsDeleted
                                             && zc.IsActive && !zc.IsDeleted
                                             && c.IsActive && !c.IsDeleted
                                             && zpv.IsActive && !zpv.IsDeleted
                                              && r.IsActive && !r.IsDeleted
                                              && cp.IsActive && !cp.IsDeleted
                                             && r.MerchantId == exCond.MerchantId
                                             select new
                                             {
                                                 r.ExpressId,
                                                 zc.CountryId,
                                                 zpv.ProvinceId,
                                                 p.WeightFrom,
                                                 p.WeightTo,
                                                 p.Price,
                                                 p.ZoneId,
                                                 z.RemarkTransId,
                                                 cp,
                                                 z
                                             }).ToList();




                    var originalExpressPriceVwLst = expressPriceVwLst.Where(p => p.WeightFrom <= totalWeight && p.WeightTo > totalWeight).ToList();//获取原重量的运费


                    var discountList = (from v in originalExpressPriceVwLst
                                        join d in baseRepository.GetList<ExpressDiscount>() on v.ExpressId equals d.ExpressId
                                        where d.IsActive && !d.IsDeleted
                                        && exCond.ItemAmount >= d.DiscountMoney
                                        && d.MerchantId == exCond.MerchantId
                                        select d).ToList();
                    ExpressDiscount maxDiscount = null;
                    if (discountList?.Count > 0)
                    {
                        maxDiscount = discountList.OrderByDescending(x => x.DiscountMoney).FirstOrDefault();
                    }


                    var notECSData = new List<ExpressChargeInfo>();
                    foreach (var item in originalExpressPriceVwLst)
                    {

                        if (item != null)
                        {

                            decimal deliveryCharge = 0;
                            decimal originalDeliveryCharge = 0;


                            var actualPostage = CalculateProductWeight(new PostageInfo(), exCond, item.cp.Code);//如果有免运费的产品，则重新计算运费

                            if (actualPostage.Weight != totalWeight)
                            {
                                if (actualPostage.Weight > 0)
                                {
                                    deliveryCharge = expressPriceVwLst.FirstOrDefault(p => p.ExpressId == item.ExpressId && p.WeightFrom <= actualPostage.Weight && p.WeightTo > actualPostage.Weight)?.Price ?? 0;
                                    originalDeliveryCharge = item.Price;
                                }
                                else
                                {
                                    deliveryCharge = 0;
                                    originalDeliveryCharge = item.Price;
                                }
                            }
                            else
                            {
                                deliveryCharge = item.Price;
                                originalDeliveryCharge = item.Price;
                            }

                            var express = new ExpressChargeInfo();
                            express.Id = Guid.NewGuid();
                            express.ExpressCompanyId = item.ExpressId;
                            express.Price = deliveryCharge * (100 + item.z.FuelSurcharge) / 100M;
                            express.OriginalPrice = originalDeliveryCharge * (100 + item.z.FuelSurcharge) / 100M;

                            if (maxDiscount != null)
                            {
                                if (maxDiscount.IsPercent)
                                {
                                    express.Discount = express.Price * maxDiscount.DiscountPercent / 100M;
                                }
                                else
                                {
                                    express.Discount = maxDiscount.DiscountPercent;
                                }
                            }
                            else
                            {
                                express.Discount = 0;
                            }
                            //express.Discount = 0;

                            express.ExpressCompanyName = GetMultiLangVal(GetMultiLangList(item.cp.NameTransId));
                            express.CountryCode = baseRepository.GetModel<Country>(p => p.Id == item.CountryId)?.Code ?? "";
                            express.ServiceType = item.cp.Code ?? "";

                            string md5Formate = "{0}{1}{2}{3}";
                            md5Formate = string.Format(md5Formate, express.Id, express.Price.ToString("N4"), express.Discount.ToString("N4"), StoreConst.DeliveryPriceSalt);
                            express.Vcode = HashUtil.MD5(md5Formate);
                            notECSData.Add(express);
                        }
                    }


                    SystemResult ECSResult = new SystemResult();
                    //需要增加ECS部分調用API獲取到的運費列表
                    List<ExpressChargeInfo> ECSData = new List<ExpressChargeInfo>();

                    try
                    {
                        if (exCond.TotalWeight > 0)
                        {
                            ECSResult = GetECSDeliveryCharge(exCond);
                            if (ECSResult.Succeeded)
                            {
                                ECSData = (List<ExpressChargeInfo>)ECSResult.ReturnValue;
                            }
                            else
                            {
                                result.Message = ECSResult.Message;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        result.Message = Resources.Message.ECShipChargeError;
                    }

                    data.AddRange(notECSData);
                    data.AddRange(ECSData);

                    data = GetActiveExpressChargeInfo(data, exCond.MerchantId);
                }

                result.ReturnValue = data;
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
                result.ReturnValue = null;
                //throw ex;
            }

            return result;
        }

        public PageData<ExpressCompanyView> GetExpress(ExpressSearchCond cond)
        {
            PageData<ExpressCompanyView> data = new PageData<ExpressCompanyView>();

            var fromIndex = ((cond.PageInfo.Page - 1) * cond.PageInfo.PageSize) + 1;
            var toIndex = cond.PageInfo.Page * cond.PageInfo.PageSize;

            StringBuilder sb = new StringBuilder();
            List<SqlParameter> paramList = new List<SqlParameter>();
            sb.AppendLine("select CCode, TCode, Code,[Name],Id,CreateDate,IsActive ");
            sb.AppendLine(" from(");

            sb.AppendLine(" select ROW_NUMBER() OVER(ORDER BY Code) as rowNum");

            sb.AppendLine(",a.* from(");

            if (cond.MerchantId != Guid.Empty)
            {


                sb.AppendLine(" select distinct e.CCode,e.TCode,e.Code,t.Value [Name],e.Id,e.CreateDate,e.IsActive from ExpressCompanies e");
                sb.AppendLine(" left join Translations t on e.NameTransId = t.TransId ");
                sb.AppendLine(" and t.Lang=@Lang and  t.IsActive=1 and t.IsDeleted=0");
                sb.AppendLine(" left join MerchantActiveShipMethods m on m.MerchantId = @MerchantId ");
                sb.AppendLine(" and m.ShipCode=e.Code and t.IsActive=1 and t.IsDeleted=0");
                paramList.Add(new SqlParameter("@MerchantId", cond.MerchantId));
            }
            else
            {
                sb.AppendLine(" select e.CCode,e.TCode,e.Code,t.Value [Name],e.Id,e.CreateDate,e.IsActive from ExpressCompanies e");
                sb.AppendLine(" left join Translations t on e.NameTransId = t.TransId ");
                sb.AppendLine(" and t.Lang=@Lang and t.IsActive=1 and t.IsDeleted=0");
            }

            sb.AppendLine(" where 1 = 1 and e.IsDeleted = 0 ");

            paramList.Add(new SqlParameter("@Lang", CurrentUser.Lang));
            if (!string.IsNullOrEmpty(cond.CCode))
            {
                sb.AppendLine(" and UPPER(e.CCode) like @CCode ");
                paramList.Add(new SqlParameter("@CCode", "%" + cond.CCode + "%"));
            }
            if (!string.IsNullOrEmpty(cond.TCode))
            {
                sb.AppendLine(" and UPPER(e.TCode) like @TCode ");
                paramList.Add(new SqlParameter("@TCode", "%" + cond.TCode + "%"));
            }

            if (!string.IsNullOrEmpty(cond.Code))
            {
                sb.AppendLine(" and UPPER(e.Code) like @Code ");
                paramList.Add(new SqlParameter("@Code", "%" + cond.Code + "%"));
            }

            if (!string.IsNullOrEmpty(cond.Name))
            {
                sb.AppendLine(" and t.Value like @Name ");
                paramList.Add(new SqlParameter("@Name", "%" + cond.Name + "%"));
            }


            sb.AppendLine(" ) a");

            sb.AppendLine(" ) b");
            data.TotalRecord = UnitOfWork.DataContext.Database.SqlQuery<ExpressCompanyView>(sb.ToString(), paramList.ToArray()).Count();


            if (toIndex > 0 && toIndex >= fromIndex)
            {
                sb.AppendFormat(" where rowNum between @FromIndex and @ToIndex", fromIndex, toIndex);
                paramList.Add(new SqlParameter("@FromIndex", fromIndex));
                paramList.Add(new SqlParameter("@ToIndex", toIndex));
            }
            var paramList2 = paramList.Select(x => ((ICloneable)x).Clone());

            var result = UnitOfWork.DataContext.Database.SqlQuery<ExpressCompanyView>(sb.ToString(), paramList2.ToArray()).ToList();
            if (result.Count() > 0)
            {
                data.Data = result;
            }

            return data;
        }

        /// <summary>
        /// 计算ECship的运费
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="provinveId"></param>
        /// <param name="totalWeight"></param>
        /// <returns></returns>
        private SystemResult GetECSDeliveryCharge(ExpressCondition exCond)
        {
            SystemResult result = new SystemResult();
            List<ExpressChargeInfo> list = new List<ExpressChargeInfo>();
            try
            {
                var mbrAddress = _deliveryAddressBLL.GetAddress(exCond.DeliveryAddrId);
                if (mbrAddress != null)
                {
                    var localCountry = _codeMasterBLL.GetCodeMasterByKey(CodeMasterModule.Setting.ToString(), CodeMasterFunction.ECShip.ToString(), "LocalCountry")?.Value ?? "HKG";


                    int countryId = 0;
                    string addressCountry = "";
                    if (mbrAddress.CountryId != 0)
                    {
                        countryId = mbrAddress.CountryId;
                        addressCountry = baseRepository.GetModel<Country>(p => p.Id == countryId)?.Code ?? "";
                    }
                    int provinceId = 0;
                    if (mbrAddress.ProvinceId != 0)
                    {
                        provinceId = mbrAddress.ProvinceId;
                    }


                    var expresss = (from e in baseRepository.GetList<ExpressCompany>()
                                    join z in baseRepository.GetList<ExpressZone>() on e.Id equals z.ExpressId
                                    join m in baseRepository.GetList<ExpressZoneCountry>() on z.Id equals m.ZoneId
                                    join c in baseRepository.GetList<Country>() on m.CountryId equals c.Id
                                    join p in baseRepository.GetList<ExpressZoneProvince>() on z.Id equals p.ZoneId
                                    where e.Code == ExpressCompanyEnum.ECS.ToString()
                                    && m.CountryId == countryId
                                    && p.ProvinceId == provinceId
                                    //&& (z.zone_name_c == address.CityName || z.zone_name_e == address.CityName || z.zone_name_j == address.CityName || z.zone_name_s == address.CityName)
                                    select new ECShipZoneInfo
                                    {
                                        ExpressCompanyId = e.Id,
                                        CountryCode = c.Code,
                                        ZoneCode = z.Code,
                                        ExpressCompanyNameId = e.NameTransId,
                                        ExpressCompanyName = ""
                                    }
                            ).ToList();

                    //foreach (var item in expresss)
                    //{
                    //    item.ExpressCompanyNameId = _translationRepository.GetMutiLanguage(item.ExpressCompanyNameId).FirstOrDefault(p => p.Language == ReturnDataLanguage)?.Desc ?? "";
                    //}
                    List<ECShipZoneInfo> zones = new List<ECShipZoneInfo>();
                    if (addressCountry == "CNA")//如果属性中国
                    {
                        if (expresss == null || expresss.Count == 0)//且不包含在指定的区域中，ZoneCode默认为CNG(其它区域)
                        {
                            zones = (from e in baseRepository.GetList<ExpressCompany>()
                                     where e.Code == ExpressCompanyEnum.ECS.ToString()
                                     select new ECShipZoneInfo
                                     {
                                         ExpressCompanyId = e.Id,
                                         CountryCode = "CNG",
                                         ZoneCode = "CNG",
                                         ExpressCompanyNameId = e.NameTransId,
                                         ExpressCompanyName = ""
                                     }
                            ).ToList();
                        }
                        else
                        {
                            zones = expresss;
                        }
                    }
                    else
                    {
                        if (expresss.Count > 0)//中国香港
                        {
                            zones = expresss;
                        }
                        else//外国
                        {
                            zones = (from e in baseRepository.GetList<ExpressCompany>()
                                     where e.Code == ExpressCompanyEnum.ECS.ToString()
                                     select new ECShipZoneInfo
                                     {
                                         ExpressCompanyId = e.Id,
                                         CountryCode = addressCountry,
                                         ZoneCode = addressCountry,
                                         ExpressCompanyNameId = e.NameTransId,
                                         ExpressCompanyName = ""
                                     }).ToList();
                        }
                    }


                    var accountInfo = _settingBLL.GetECShipAccountInfo();

                    //var merchant = MerchantBLL.GetMerchById(exCond.MerchantId);//現在ECShipName用mechant的ECShipCode代替

                    //accountInfo.ECShipName = merchant.ECShipCode;


                    //如果Zone的Code为Other时，用Country Code作为CountryCode，Code作为CountryCode
                    foreach (var item in zones)
                    {
                        PostageInfo originalPostage = new PostageInfo();
                        List<string> shipCodes = new List<string>();
                        originalPostage.Weight = exCond.TotalWeight;
                        if (item.ZoneCode.Trim().Length > 3 && item.ZoneCode.Trim().ToUpper() == "OTHER")
                        {
                            originalPostage.CountryCode = item.CountryCode;
                        }
                        else
                        {
                            originalPostage.CountryCode = item.ZoneCode;
                        }

                        if (localCountry.Trim().ToUpper() == item.CountryCode.Trim().ToUpper())//如果是本土国家
                        {
                            originalPostage.MailType = "D1";
                            if (exCond.TotalWeight > 2 && exCond.TotalWeight < 20)
                            {
                                shipCodes.Add("LPL");
                            }
                            else
                            {
                                //shipCodes.Add("SMP");//已廢棄，使用LEG(易寄取)代替
                                shipCodes.Add("LEG");
                            }
                        }
                        else
                        {
                            originalPostage.MailType = "E";
                            if (exCond.TotalWeight > 2 && exCond.TotalWeight <= 30)
                            {
                                shipCodes.Add("EMS");
                            }
                            else if (exCond.TotalWeight <= 2)
                            {
                                shipCodes.Add("ARM");
                                shipCodes.Add("EMS");
                            }

                        }

                        if (shipCodes.Count > 0)
                        {
                            foreach (var postItem in shipCodes)
                            {
                                try
                                {
                                    #region
                                    originalPostage.ShipCode = postItem;
                                    var originalTotalPostage = Calculator.GetTotalPostage(accountInfo, originalPostage);

                                    var shipTypeCode = "";
                                    decimal deliveryCharge = 0;
                                    decimal originalDeliveryCharge = 0;
                                    TotalPostageReturnInfo actualTotalPostage = null;

                                    if (postItem == "EMS")//获取shipmethod的Code
                                    {
                                        //if (originalPostage.CountryCode == "CNC")
                                        //{
                                        //    shipTypeCode = "GDEMS";
                                        //}
                                        //else if (originalPostage.CountryCode != "CNC")
                                        //{
                                        //    shipTypeCode = "StdEMS";
                                        //}
                                        shipTypeCode = "StdEMS";
                                    }
                                    else
                                    {
                                        shipTypeCode = postItem;
                                    }

                                    var actualPostage = CalculateProductWeight(originalPostage, exCond, shipTypeCode);//计算实际重量

                                    if (originalPostage.Weight != actualPostage.Weight)
                                    {
                                        if (actualPostage.Weight > 0)
                                        {
                                            actualTotalPostage = Calculator.GetTotalPostage(accountInfo, actualPostage);
                                        }
                                        deliveryCharge = actualPostage.Weight == 0 ? 0 : actualTotalPostage.TotalPostage;
                                        originalDeliveryCharge = originalTotalPostage.TotalPostage;
                                    }
                                    else
                                    {
                                        deliveryCharge = originalTotalPostage.TotalPostage;
                                        originalDeliveryCharge = originalTotalPostage.TotalPostage;
                                    }

                                    if (originalTotalPostage.Status == "0")
                                    {
                                        list.Add(new ExpressChargeInfo
                                        {
                                            Id = Guid.NewGuid(),
                                            CountryCode = originalPostage.CountryCode,
                                            ShipCode = originalPostage.ShipCode,
                                            ServiceType = postItem,
                                            ExpressCompanyId = item.ExpressCompanyId,
                                            ExpressCompanyName = GetExpressCompanyName(item.ExpressCompanyId, postItem, originalPostage.CountryCode),
                                            Price = deliveryCharge,//originalTotalPostage.TotalPostage,
                                            OriginalPrice = originalDeliveryCharge,
                                            Discount = GetECShipDiscount(item.ExpressCompanyId, exCond.ItemAmount, deliveryCharge)
                                        });
                                    }
                                    else
                                    {
                                        //result.Message = Resources.Message.ECShipChargeError;
                                        //SaveInfo(GetType().FullName, ClassUtility.GetMethodName(), "ECS GetTotalPostage return Status= " + originalTotalPostage.Status, originalTotalPostage.ErrMessage);
                                    }
                                    #endregion
                                }
                                catch (Exception exShipCode)
                                {
                                    //SaveError(GetType().FullName, ClassUtility.GetMethodName(), "ECS API獲取運費異常，Shipcode：" + postItem, exShipCode);
                                    continue;
                                }
                            }

                            if (string.IsNullOrEmpty(result.Message))
                            {
                                result.Succeeded = true;
                                result.ReturnValue = list;
                                result.Message = "";
                            }
                            else
                            {
                                result.Succeeded = false;
                                result.ReturnValue = list;
                                result.Message = Resources.Message.ECShipChargeError;
                            }

                        }


                    }
                }


                foreach (var item in list)
                {
                    string md5Formate = "{0}{1}{2}{3}";
                    md5Formate = string.Format(md5Formate, item.Id, item.Price.ToString("N4"), item.Discount.ToString("N4"), StoreConst.DeliveryPriceSalt);
                    item.Vcode = HashUtil.MD5(md5Formate);
                }
            }
            catch (Exception ex)
            {
                result.Message = Resources.Message.ECShipChargeError;
                result.Succeeded = false;
                result.ReturnValue = list;
                //SaveError(GetType().FullName, ClassUtility.GetMethodName(), "", ex.Message);
            }

            return result;

        }

        /// <summary>
        /// 根據商家設置的有效付運來篩選運費
        /// </summary>
        /// <param name="chargeInfos"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        private List<ExpressChargeInfo> GetActiveExpressChargeInfo(List<ExpressChargeInfo> chargeInfos, Guid merchantId)
        {
            List<ExpressChargeInfo> result = new List<ExpressChargeInfo>();
            var activeShipMethod = _merchantBLL.GetMerchantShipMethods(merchantId);

            if (activeShipMethod.MerchantShipMethods != null)
            {
                foreach (var item in chargeInfos)
                {
                    string type = "";
                    if (item.ServiceType != "EMS")
                    {
                        type = item.ServiceType;
                    }
                    else
                    {
                        type = "StdEMS";
                    }

                    var shipMethod = activeShipMethod.MerchantShipMethods.FirstOrDefault(p => p.ShipMethodCode == type && p.IsEffect == true);
                    if (shipMethod != null)
                    {
                        result.Add(item);
                    }
                }
            }
            return result;

        }
        private PostageInfo CalculateProductWeight(PostageInfo originalPostage, ExpressCondition exCond, string shipCode)
        {

            PostageInfo postageInfo = new PostageInfo();
            postageInfo.CountryCode = originalPostage.CountryCode;
            postageInfo.MailType = originalPostage.MailType;
            postageInfo.ShipCode = originalPostage.ShipCode;
            postageInfo.Weight = 0;
            if (exCond.ProductWeightInfo != null)
            {
                var freeCharge = _merchantFreeChargeRepository.GetByMerchantId(exCond.MerchantId);

                foreach (var item in exCond.ProductWeightInfo)
                {
                    var freeProduct = freeCharge.FirstOrDefault(p => p.ProductCode == item.Code && p.ShipCode == shipCode);

                    if (freeProduct == null)
                    {
                        var productId = _productRepository.GetOnSaleProductId(item.Code);
                        var productSpecification = baseRepository.GetModel<ProductSpecification>(p => p.Id == productId);
                        if (productSpecification != null)
                        {
                            decimal weight = 0;
                            if (productSpecification.WeightUnit == WeightUnit.G)
                            {
                                weight = productSpecification.GrossWeight / 1000;
                            }
                            else
                            {
                                weight = productSpecification.GrossWeight;
                            }
                            postageInfo.Weight += weight * item.Qty;
                        }
                    }
                }
            }
            else
            {
                postageInfo.Weight = exCond.TotalWeight;
            }
            return postageInfo;


        }
        private string GetExpressCompanyName(Guid expressId, string code, string CountryCode)
        {

            var expressName = _translationRepository.GetMutiLanguage(expressId).FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "";

            var serviceName = "";


            if (code == "EMS")
            {
                code = "StdEMS";
            }

            serviceName = _codeMasterBLL.GetCodeMaster(CodeMasterModule.System, CodeMasterFunction.ECShipService, code)?.Description ?? "";

            return expressName + " " + serviceName;

        }

        private decimal GetECShipDiscount(Guid expressID, decimal totalAmount, decimal deliveryCharge)
        {
            decimal discount = 0;

            var query = (from e in baseRepository.GetList<ExpressCompany>()
                         join d in baseRepository.GetList<ExpressDiscount>() on e.Id equals d.ExpressId
                         where d.IsActive && !d.IsDeleted && e.Id == expressID && totalAmount >= d.DiscountMoney
                         select d).FirstOrDefault();

            if (query != null)
            {
                if (query.IsPercent)
                {
                    discount = deliveryCharge * (query.DiscountPercent / 100);
                }
                else
                {
                    discount = query.DiscountPercent;
                }
            }
            return discount;

        }
        private decimal GetSFDiscount(string Code, Guid merchId, decimal totalAmount, decimal deliveryCharge)
        {
            decimal discount = 0;

            var query = (from e in baseRepository.GetList<ExpressCompany>()
                         join d in baseRepository.GetList<ExpressDiscount>() on e.Id equals d.ExpressId
                         where d.IsActive && !d.IsDeleted && e.Code == Code && totalAmount >= d.DiscountMoney
                         select d).FirstOrDefault();

            if (query != null)
            {
                if (query.IsPercent)
                {
                    discount = deliveryCharge * (query.DiscountPercent / 100);
                }
                else
                {
                    discount = query.DiscountPercent;
                }
            }
            return discount;

        }
        public List<KeyValue> GetExpressForSelect()
        {
            var data = GetExpress();
            List<KeyValue> list = data.Select(d => new KeyValue
            {
                Id = d.Id.ToString(),
                Text = d.Name,
            }).ToList();
            return list;
        }

        public ExpressCompanyDto GetExpressItem(Guid id)
        {
            var langs = _settingBLL.GetSupportLanguages();
            ExpressCompany item = baseRepository.GetModel<ExpressCompany>(d => d.Id == id && d.IsDeleted == false);
            var dto = AutoMapperExt.MapTo<ExpressCompanyDto>(item);
            if (dto != null)
            {
                dto.Names = _translationRepository.GetMutiLanguage(item.NameTransId);
                dto.CountryIds = baseRepository.GetList<ExpressCountry>().Where(d => d.ExpressId == item.Id).Select(d => d.CountryId).ToList();
                dto.Name = dto.Names.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "";
            }
            else
            {
                dto = new ExpressCompanyDto();
                dto.Names = LangUtil.GetMutiLangFromTranslation(null, langs);
                dto.Name = "";
            }
            return dto;
        }

        public ExpressCompanyDto GetExpressItem(string code)
        {
            var langs = _settingBLL.GetSupportLanguages();
            ExpressCompany item = baseRepository.GetModel<ExpressCompany>(d => d.Code == code && d.IsDeleted == false);
            var dto = AutoMapperExt.MapTo<ExpressCompanyDto>(item);
            if (dto != null)
            {
                dto.Names = _translationRepository.GetMutiLanguage(item.NameTransId);
                dto.CountryIds = baseRepository.GetList<ExpressCountry>().Where(d => d.ExpressId == item.Id).Select(d => d.CountryId).ToList();
                dto.Name = dto.Names.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "";
            }
            else
            {
                dto = new ExpressCompanyDto();
                dto.Names = LangUtil.GetMutiLangFromTranslation(null, langs);
                dto.Name = "";
            }
            return dto;
        }

        public List<TransPriceInfo> GetExpressPrice(Guid id)
        {
            List<TransPriceInfo> list = new List<TransPriceInfo>();
            var zones = baseRepository.GetList<ExpressPrice>().Where(d => d.RuleId == id);
            list = zones.GroupBy(d => d.WeightTo).Select(b => new TransPriceInfo
            {
                Weight = b.Key,
                zoneCharge = b.ToList(),
            }).ToList();
            return list;
        }

        public List<ExpressRule> GetExpressRule(Guid id, Guid merchId)
        {
            List<ExpressRule> list = baseRepository.GetList<ExpressRule>().Where(d => d.ExpressId == id && d.MerchantId == merchId && d.IsDeleted == false).OrderBy(d => d.Seq).ToList();
            return list;
        }

        public List<SystemLang> GetLangs()
        {
            return _settingBLL.GetSupportLanguages();
        }


        /// <summary>
        /// 獲取快遞目前最大重量
        /// </summary>
        /// <param name="exId">快遞編號</param>
        /// <returns></returns>
        public decimal GetMaxWeightByExpress(Guid exId, Guid merchId)
        {
            decimal MaxWeight;
            var rules = baseRepository.GetList<ExpressRule>().Where(d => d.ExpressId == exId && d.MerchantId == merchId && d.IsDeleted == false).ToList();
            if (rules.Count > 0)
            {
                MaxWeight = rules.Max(d => d.WeightTo);
            }
            else
            {
                MaxWeight = 0;
            }
            return MaxWeight;
        }

        /// <summary>
        /// 根据商家Id获取对应运输方式
        /// </summary>
        /// <param name="merchId"></param>
        /// <returns></returns>
        public List<ExpressCompanyDto> GetMerchantExpress(Guid merchId)
        {
            List<ExpressCompanyDto> result = new List<ExpressCompanyDto>();


            var langs = _settingBLL.GetSupportLanguages();
            var list = baseRepository.GetList<ExpressCompany>().Where(d => d.IsDeleted == false && d.IsActive).ToList();

            var dtos = AutoMapperExt.MapToList<ExpressCompany, ExpressCompanyDto>(list);
            if (merchId == Guid.Empty)
            {
                foreach (var item in dtos)
                {
                    item.Names = _translationRepository.GetMutiLanguage(item.NameTransId);
                    item.Name = langs == null ? "" : item.Names.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc;

                    result.Add(item);
                }
            }
            else
            {
                var dbShipmethods = _merchantShipMethodMappingRepository.GetShipMethidByMerchantId(merchId).OrderBy(o => o.ShipCode).ToList();
                foreach (var item in dtos)
                {
                    var merchRecord = dbShipmethods.FirstOrDefault(p => p.ShipCode == item.Code);
                    if (merchRecord != null)
                    {
                        item.Names = _translationRepository.GetMutiLanguage(item.NameTransId);
                        item.Name = langs == null ? "" : item.Names.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc;
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        public List<KeyValue> GetMerchantExpress()
        {
            var merchId = CurrentUser.MerchantId;
            var data = GetMerchantExpress(merchId);
            List<KeyValue> list = data.Select(d => new KeyValue
            {
                Id = d.Id.ToString(),
                Text = d.Name,
            }).ToList();
            return list;
        }

        public List<KeyValue> GetMerchantExpressByCond(Guid merchId)
        {
            var data = GetMerchantExpress(merchId);
            List<KeyValue> list = data.Select(d => new KeyValue
            {
                Id = d.Id.ToString(),
                Text = d.Name,
            }).ToList();
            return list;
        }

        public KeyValue GetMerchantNameBySelect(Guid id)
        {
            var MerchantName = GetMerchantName(id);

            KeyValue list = new KeyValue();
            list.Id = id.ToString();
            list.Text = BDMall.Resources.Value.Mall;
            if (MerchantName == "")
                list.Text = BDMall.Resources.Value.Mall;
            else
                list.Text = MerchantName;
            return list;
        }
        /// <summary>
        /// 獲取商家名
        /// </summary>
        /// <param name="id">快遞ID</param>
        /// <returns></returns>
        private string GetMerchantName(Guid id)
        {


            var transId = baseRepository.GetModel<Merchant>(p => p.Id == id)?.NameTransId ?? new Guid();
            if (transId != Guid.Empty)
            {
                var name = _translationRepository.GetMutiLanguage(transId)?.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "";
                return name;
            }
            return "";
        }
        public List<ProvinceDto> GetProvinceByCountry(int countryId)
        {
            var langs = _settingBLL.GetSupportLanguages();
            var list = _provinceRepository.GetListByCountry(countryId);
            foreach (var item in list)
            {
                item.Name = NameUtil.GetProviceName(CurrentUser.Lang.ToString(), item);
                item.Names = LangUtil.GetMutiLang<ProvinceDto>(item, "Name", langs);
                item.Cities = GetCityByProvince(item.Id);
            }
            return list;
        }
        private List<CityDto> GetCityByProvince(int provinceId)
        {
            var langs = _settingBLL.GetSupportLanguages();
            var list = _cityRepository.GetListByProvince(provinceId);
            foreach (var item in list)
            {
                item.Name = NameUtil.GetCityName(CurrentUser.Lang.ToString(), item);
                item.Names = LangUtil.GetMutiLang<CityDto>(item, "Name", langs);
            }
            return list;
        }
        public List<KeyValue> GetProvinceByCountryForSelect(int countryId)
        {
            var list = GetProvinceByCountry(countryId).Select(d => new KeyValue
            {
                Id = d.Id.ToString(),
                Text = d.Name,
            }).ToList();
            return list;
        }

        public List<KeyValue> GetProvinceByCountryZoneForSelect(int countryId, Guid zoneid, Guid id)
        {
            List<int> selectedProvince = new List<int>();
            var orderZone = baseRepository.GetList<ExpressZone>().Where(d => d.ExpressId == id && !d.Id.Equals(zoneid) && d.IsDeleted == false).Select(d => d.Id).ToList();
            if (orderZone?.Count() > 0)
            {
                selectedProvince = baseRepository.GetList<ExpressZoneProvince>().Where(d => d.IsDeleted == false && d.CountryId == countryId && orderZone.Contains(d.ZoneId)).Select(d => d.ProvinceId).ToList();
            }
            var list = baseRepository.GetList<Province>().Where(d => d.IsDeleted == false && d.CountryId == countryId && !selectedProvince.Contains(d.Id)).ToList();
            var dtos = AutoMapperExt.MapToList<Province, ProvinceDto>(list);
            foreach (var item in dtos)
            {
                item.Name = NameUtil.GetProviceName(CurrentUser.Lang.ToString(), item);
            }
            var keyList = list.Select(d => new KeyValue
            {
                Id = d.Id.ToString(),
                Text = d.Name,
            }).ToList();
            return keyList;
        }

        public ProvinceDto GetProvinceItem(int id)
        {
            return _provinceRepository.GetById(id);
        }

        public ExpressRule GetRuleItem(Guid id)
        {
            ExpressRule obj = new ExpressRule();
            var item = baseRepository.GetModel<ExpressRule>(p => p.Id == id);
            if (item != null)
            {
                obj = item;
            }
            return obj;
        }

        public List<ExpressZoneDto> GetZone(Guid id)
        {
            var langs = _settingBLL.GetSupportLanguages();
            var name = GetExpressName(id);
            List<ExpressZone> list = baseRepository.GetList<ExpressZone>().Where(p => p.ExpressId == id && p.IsActive && !p.IsDeleted).ToList();

            var dtos = AutoMapperExt.MapToList<ExpressZone, ExpressZoneDto>(list);

            foreach (var item in dtos)
            {
                item.Names = _translationRepository.GetMutiLanguage(item.NameTransId);
                item.Name = langs == null ? "" : item.Names.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc;
                item.Remarks = _translationRepository.GetMutiLanguage(item.RemarkTransId);
                item.Remark = langs == null ? "" : item.Remarks.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc;
                item.ExpressCompanyName = name;
            }
            return dtos;
        }

        /// <summary>
        /// 獲取快遞名
        /// </summary>
        /// <param name="id">快遞ID</param>
        /// <returns></returns>
        private string GetExpressName(Guid id)
        {


            var transId = baseRepository.GetModel<ExpressCompany>(p => p.Id == id)?.NameTransId ?? new Guid();
            if (transId != Guid.Empty)
            {
                var name = _translationRepository.GetMutiLanguage(transId)?.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "";
                return name;
            }
            return "";
        }

        public List<KeyValue> GetZoneForSelect(Guid id)
        {
            var data = GetZone(id);
            List<KeyValue> list = data.Select(d => new KeyValue
            {
                Id = d.Id.ToString(),
                Text = d.Name,
            }).ToList();
            return list;
        }

        public ZoneInfo GetZoneItem(Guid id)
        {
            var langs = _settingBLL.GetSupportLanguages();
            ZoneInfo zoneInfo = new ZoneInfo();
            ExpressZoneDto obj = new ExpressZoneDto();
            List<exCounProvince> exCP = new List<exCounProvince>();
            int[] exC = { 0 };
            var item = baseRepository.GetModel<ExpressZone>(d => d.Id == id && d.IsDeleted == false);
            if (item != null)
            {
                obj.Id = id;
                obj.Code = item.Code;
                obj.FuelSurcharge = item.FuelSurcharge;
                obj.ExpressId = item.ExpressId;
                obj.Names = _translationRepository.GetMutiLanguage(item.NameTransId);
                obj.Remarks = _translationRepository.GetMutiLanguage(item.RemarkTransId);
                exC = baseRepository.GetList<ExpressZoneCountry>().Where(d => d.ZoneId == id).Select(d => d.CountryId).ToArray();
                if (exC.Count() > 0)
                {
                    exCP = GetExCounProvince(id, exC);
                }
            }
            else
            {
                obj.Id = new Guid();
                obj.Code = "";
                obj.FuelSurcharge = 0;
                obj.Names = LangUtil.GetMutiLangFromTranslation(null, langs);
                obj.Remarks = LangUtil.GetMutiLangFromTranslation(null, langs);
                obj.ExpressId = new Guid();
            }
            zoneInfo.exCounProvince = exCP;
            zoneInfo.zone = obj;
            zoneInfo.exCountry = exC;
            return zoneInfo;
        }
        /// <summary>
        /// 獲取該區域服務範圍
        /// </summary>
        /// <param name="zoneId">區域編號</param>
        /// <param name="exC">快遞適用國家</param>
        /// <returns></returns>
        private List<exCounProvince> GetExCounProvince(Guid zoneId, int[] exC)
        {
            List<exCounProvince> list = new List<exCounProvince>();
            foreach (var Countryid in exC)
            {
                string[] provinceArr = baseRepository.GetList<ExpressZoneProvince>().Where(d => d.ZoneId == zoneId && d.CountryId == Countryid).Select(d => d.ProvinceId.ToString()).ToArray();
                list.Add(new exCounProvince()
                {
                    country = Countryid,
                    province = provinceArr,
                });
            }
            return list;
        }

        public SystemResult SaveCountry(CountryDto model)
        {
            SystemResult result = new SystemResult();
            //UnitOfWork.IsUnitSubmit = true;//统一提交

            if (StringHtmlContentCheck(model))
            {
                result.Succeeded = false;
                result.Message = Resources.Message.ExistHTMLLabel;

                return result;
            }

            #region check
            model.Name_e = (model.Names?.FirstOrDefault(d => d.Lang.Code == "E")?.Desc ?? "").Trim();
            model.Name_c = (model.Names?.FirstOrDefault(d => d.Lang.Code == "C")?.Desc ?? "").Trim();
            model.Name_s = (model.Names?.FirstOrDefault(d => d.Lang.Code == "S")?.Desc ?? "").Trim();
            model.Name_j = (model.Names?.FirstOrDefault(d => d.Lang.Code == "J")?.Desc ?? "").Trim();
            #endregion
            if (model?.Id != 0)
            {
                var entity = baseRepository.GetModel<Country>(p => p.Id == model.Id);
                entity.Name_e = model.Name_e;
                entity.Name_c = model.Name_c;
                entity.Name_s = model.Name_s;
                entity.Name_j = model.Name_j;
                entity.Code = model.Code ?? "";
                entity.IsNeedPostalCode = model.IsNeedPostalCode;
                entity.Seq = model.Seq;
                baseRepository.Update(entity);

                if (model.Procince?.Count() > 0)
                {
                    SaveProvince(model.Procince, model.Id);
                }
            }
            else
            {
                var dbModel = AutoMapperExt.MapTo<Country>(model);
                dbModel.CreateBy = Guid.Parse(CurrentUser.UserId);
                dbModel.CreateDate = DateTime.Now;
                dbModel.IsActive = true;
                dbModel.IsDeleted = false;
                baseRepository.Insert(dbModel);
                if (model.Procince?.Count() > 0)
                {
                    model.Id = baseRepository.GetList<Country>().Max(d => d.Id);
                    SaveProvince(model.Procince, model.Id);
                }
            }

            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.UpdateSuccess;

            return result;
        }
        private void SaveProvince(List<ProvinceDto> list, int countryId)
        {
            foreach (var model in list)
            {
                if (StringHtmlContentCheck(model))
                {
                    throw new BLException(Resources.Message.ExistHTMLLabel);
                }

                #region check
                model.Name_e = (model.Names?.FirstOrDefault(d => d.Lang.Code == "E")?.Desc ?? "").Trim();
                model.Name_c = (model.Names?.FirstOrDefault(d => d.Lang.Code == "C")?.Desc ?? "").Trim();
                model.Name_s = (model.Names?.FirstOrDefault(d => d.Lang.Code == "S")?.Desc ?? "").Trim();
                model.Name_j = (model.Names?.FirstOrDefault(d => d.Lang.Code == "J")?.Desc ?? "").Trim();
                #endregion
                if (model?.Id != 0)
                {
                    var entity = baseRepository.GetModel<Province>(p => p.Id == model.Id);
                    entity.Name_e = model.Name_e;
                    entity.Name_c = model.Name_c;
                    entity.Name_s = model.Name_s;
                    entity.Name_j = model.Name_j;
                    entity.Code = model.Code == null ? "" : model.Code;
                    entity.UpdateDate = DateTime.Now;
                    entity.UpdateBy = Guid.Parse(CurrentUser.UserId);
                    baseRepository.Update(entity);
                }
                else
                {
                    model.CountryId = countryId;
                    var dbModel = new Province();
                    dbModel.CountryId = countryId;
                    dbModel.CreateBy = Guid.Parse(CurrentUser.UserId);
                    dbModel.CreateDate = DateTime.Now;
                    dbModel.IsActive = true;
                    dbModel.IsDeleted = false;
                    dbModel.Name = model.Name;
                    dbModel.Name_e = model.Name_e;
                    dbModel.Name_c = model.Name_c;
                    dbModel.Name_s= model.Name_s;
                    dbModel.Name_j= model.Name_j;
                    dbModel.Code = model.Code;
                    baseRepository.Insert(dbModel);
                }
            }
        }
        public bool StringHtmlContentCheck(CountryDto model)
        {
            if (model != null)
            {
                if (StringUtil.CheckHasHTMLTag(model.Code))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Code2))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Code3))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Name))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Name_c))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Name_e))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Name_j))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Name_s))
                {
                    return true;
                }
                if (CheckMultLangListHasHTMLTag(model.Names))
                {
                    return true;
                }
            }

            return false;
        }

        public bool StringHtmlContentCheck(ProvinceDto model)
        {
            if (model != null)
            {
                if (StringUtil.CheckHasHTMLTag(model.Code))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Name))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Name_c))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Name_e))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Name_j))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Name_s))
                {
                    return true;
                }
                if (CheckMultLangListHasHTMLTag(model.Names))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckMultLangListHasHTMLTag(List<MutiLanguage> multList)
        {
            if (multList?.Count > 0)
            {
                foreach (var item in multList)
                {
                    if (StringUtil.CheckHasHTMLTag(item.Desc))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public SystemResult SaveDiscount(ExpressDiscount model)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;//统一提交


            if (model?.Id != Guid.Empty)
            {
                var entity = baseRepository.GetModel<ExpressDiscount>(p => p.Id == model.Id);
                entity.ExpressId = model.ExpressId;
                entity.DiscountMoney = model.DiscountMoney;
                entity.DiscountPercent = model.DiscountPercent;
                entity.IsActive = model.IsActive;
                entity.IsPercent = model.IsPercent;
                entity.MerchantId = model.MerchantId;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateBy = Guid.Parse(CurrentUser.UserId);
                baseRepository.Update(entity);
            }
            else
            {
                var dbMoel = new ExpressDiscount();
                dbMoel.Id = Guid.NewGuid();
                dbMoel.IsActive = model.IsActive;
                dbMoel.IsDeleted = false;
                dbMoel.CreateBy = Guid.Parse(CurrentUser.UserId);
                dbMoel.CreateDate = DateTime.Now;
                dbMoel.MerchantId = model.MerchantId;
                dbMoel.ExpressId = model.ExpressId;
                dbMoel.DiscountMoney = model.DiscountMoney;
                dbMoel.DiscountPercent = model.DiscountPercent;
                baseRepository.Insert(dbMoel);
            }
            UnitOfWork.Submit();
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.UpdateSuccess;

            return result;
        }

        public SystemResult SaveExpress(ExpressCompanyDto model)
        {
            SystemResult result = new SystemResult();



            UnitOfWork.IsUnitSubmit = true;//统一提交

            if (StringHtmlContentCheck(model))
            {
                result.Succeeded = false;
                result.Message = Resources.Message.ExistHTMLLabel;

                return result;
            }

            if (model?.Id != Guid.Empty)
            {
                var entity = baseRepository.GetModel<ExpressCompany>(p => p.Id == model.Id);
                entity.Code = model.Code == null ? "" : model.Code;
                // entity.Discount = model.Discount;
                entity.IsActive = model.IsActive;
                entity.CCode = model?.CCode ?? "";
                entity.TCode = model?.TCode ?? "";
                entity.UseApi = model?.UseApi ?? false;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateBy = Guid.Parse(CurrentUser.UserId);
                _translationRepository.UpdateMutiLanguage(entity.NameTransId, model.Names, TranslationType.Express);
                baseRepository.Update(entity);
                //update country
                SaveExpressCountry(model.CountryIds, model.Id);
            }
            else
            {
                var record = baseRepository.GetList<ExpressCompany>().Any(d => d.Code == model.Code);
                if (record)
                {
                    result.Succeeded = false;
                    result.Message = Resources.Message.ShipCodeIsExist;
                    return result;
                }
                foreach (var name in model.Names)
                {
                    name.Desc = name.Desc == null ? "NULL" : name.Desc;
                }

                var dbModel = new ExpressCompany();

                dbModel.Id = Guid.NewGuid();
                dbModel = AutoMapperExt.MapTo<ExpressCompany>(model);
                var NameTransId = _translationRepository.InsertMutiLanguage(model.Names, TranslationType.Express);
                dbModel.NameTransId = NameTransId;
                dbModel.IsActive = true;
                dbModel.IsDeleted = false;
                dbModel.CreateBy = Guid.Parse(CurrentUser.UserId);
                dbModel.CreateDate = DateTime.Now;
                baseRepository.Insert(dbModel);
                //update country
                SaveExpressCountry(model.CountryIds, model.Id);
            }
            UnitOfWork.Submit();
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.UpdateSuccess;

            return result;
        }
        /// <summary>
        /// 保存快遞公司適用國家
        /// </summary>
        /// <param name="countryIds"></param>
        /// <param name="expId"></param>
        private void SaveExpressCountry(List<int> countryIds, Guid expId)
        {
            var old = baseRepository.GetList<ExpressCountry>().Where(d => d.ExpressId == expId).ToList();
            if (old != null)
            {
                baseRepository.Delete(old);
            }
            foreach (var id in countryIds)
            {
                ExpressCountry conn = new ExpressCountry();
                conn.CountryId = id;
                conn.ExpressId = expId;
                conn.CreateDate = DateTime.Now;
                conn.CreateBy = Guid.Parse(CurrentUser.UserId);
                baseRepository.Insert(conn);
            }
        }
        public bool StringHtmlContentCheck(ExpressCompanyDto model)
        {
            if (model != null)
            {
                if (StringUtil.CheckHasHTMLTag(model.Code))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Name))
                {
                    return true;
                }

                if (CheckMultLangListHasHTMLTag(model.Names))
                {
                    return true;
                }
            }

            return false;
        }

        public SystemResult SaveExpressRulePrice(TransRulePrice obj)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;//统一提交

            var rule = obj.rule;
            var charge = obj.charges;
            if (charge?.Count() > 0)
            {
                if (rule.Id == Guid.Empty)
                {
                    InsertExpressRule(rule, charge);
                }
                else
                {
                    DeleteRulePrice(rule.Id);
                    InsertExpressPrice(charge, rule.Id);
                }

                UnitOfWork.Submit();
                result.Succeeded = true;
                result.Message = BDMall.Resources.Message.UpdateSuccess;
            }
            else
            {
                result.Message = BDMall.Resources.Message.AddZoneFirst;
            }
            return result;
        }
        /// <summary>
        /// 添加快遞收費規則
        /// </summary>
        /// <param name="obj">規則信息</param>
        /// <param name="charge"></param>
        private void InsertExpressRule(ExpressRule obj, List<ExpressPrice> charge)
        {
            var Seq = 1;
            var id = baseRepository.GetList<ExpressRule>().Where(d => d.ExpressId == obj.ExpressId).Select(d => d.Seq).ToArray();
            if (id.Length > 0)
            {
                Seq = baseRepository.GetList<ExpressRule>().Where(d => d.ExpressId == obj.ExpressId).Max(p => p.Seq) + 1;
            }
            ExpressRule Coun = new ExpressRule();
            Coun.Id = Guid.NewGuid();
            Coun.Seq = Seq;
            Coun.ExpressId = obj.ExpressId;
            Coun.WeightFrom = obj.WeightFrom;
            Coun.WeightTo = obj.WeightTo;
            Coun.FirstPrice = obj.FirstPrice;
            Coun.AddPrice = obj.AddPrice;
            Coun.AddWeight = obj.AddWeight;
            Coun.MerchantId = obj.MerchantId;
            Coun.CreateBy = Guid.Parse(CurrentUser.UserId);
            Coun.CreateDate = DateTime.Now;
            Coun.IsActive = true;
            Coun.IsDeleted = false;
            baseRepository.Insert(Coun);
            InsertExpressPrice(charge, Coun.Id);
        }
        /// <summary>
        /// 添加快遞價錢
        /// </summary>
        /// <param name="charges"></param>
        /// <param name="ruleId">規則編號</param>
        private void InsertExpressPrice(List<ExpressPrice> charges, Guid ruleId)
        {
            foreach (var c in charges)
            {
                ExpressPrice price = new ExpressPrice();
                price.Id = Guid.NewGuid();
                price.RuleId = ruleId;
                price.Price = c.Price;
                price.WeightFrom = c.WeightFrom;
                price.WeightTo = c.WeightTo;
                price.ZoneId = c.ZoneId;
                price.CreateBy = Guid.Parse(CurrentUser.UserId);
                price.CreateDate = DateTime.Now;
                price.IsActive = true;
                price.IsDeleted = false;
                baseRepository.Insert(price);
            }
        }
        public SystemResult SaveProvince(ProvinceDto model)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;//统一提交

            //model.Country = null;
            #region check
            model.Name_e = (model.Names?.FirstOrDefault(d => d.Lang.Code == "E")?.Desc ?? "").Trim();
            model.Name_c = (model.Names?.FirstOrDefault(d => d.Lang.Code == "C")?.Desc ?? "").Trim();
            model.Name_s = (model.Names?.FirstOrDefault(d => d.Lang.Code == "S")?.Desc ?? "").Trim();
            model.Name_j = (model.Names?.FirstOrDefault(d => d.Lang.Code == "J")?.Desc ?? "").Trim();
            #endregion
            if (model?.Id != 0)
            {
                var entity = baseRepository.GetModel<Province>(p => p.Id == model.Id);
                entity.Name_e = model.Name_e;
                entity.Name_c = model.Name_c;
                entity.Name_s = model.Name_s;
                entity.Name_j = model.Name_j;
                entity.Code = model.Code == null ? "" : model.Code;
                entity.UpdateBy = Guid.Parse(CurrentUser.UserId);
                entity.UpdateDate = DateTime.Now;
                baseRepository.Update(entity);
            }
            else
            {
                var dbModel = new Province();
                dbModel = AutoMapperExt.MapTo<Province>(model);
                dbModel.CreateBy = Guid.Parse(CurrentUser.UserId);
                dbModel.CreateDate = DateTime.Now;
                dbModel.IsActive = true;
                dbModel.IsDeleted = false;
                baseRepository.Insert(dbModel);
            }
            UnitOfWork.Submit();
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.UpdateSuccess;

            return result;
        }

        public SystemResult SaveZone(ZoneInfo zoneInfo)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;//统一提交

            var model = zoneInfo.zone;

            if (StringHtmlContentCheck(model))
            {
                result.Succeeded = false;
                result.Message = Resources.Message.ExistHTMLLabel;

                return result;
            }

            var cIds = zoneInfo.exCounProvince;
            if (model?.Id != Guid.Empty)
            {
                var entity = baseRepository.GetModel<ExpressZone>(p => p.Id == model.Id);
                entity.Code = model.Code == null ? "" : model.Code;
                entity.FuelSurcharge = model.FuelSurcharge;
                entity.ExpressId = model.ExpressId;
                _translationRepository.UpdateMutiLanguage(entity.NameTransId, model.Names, TranslationType.ExpressZone);
                _translationRepository.UpdateMutiLanguage(entity.RemarkTransId, model.Remarks, TranslationType.ExpressZone);
                baseRepository.Update(entity);
                //update zone country and procince
                InsertZoneProvince(model.Id, cIds);
                result.ReturnValue = model.Id;
                result.Message = BDMall.Resources.Message.UpdateSuccess;
            }
            else
            {
                foreach (var name in model.Names)
                {
                    name.Desc = name.Desc == null ? "" : name.Desc;
                }
                foreach (var remark in model.Remarks)
                {
                    remark.Desc = remark.Desc == null ? "" : remark.Desc;
                }
                var dbModel = AutoMapperExt.MapTo<ExpressZone>(model);
                var NameTransId = _translationRepository.InsertMutiLanguage(model.Names, TranslationType.ExpressZone);
                var RemarkTransId = _translationRepository.InsertMutiLanguage(model.Remarks, TranslationType.ExpressZone);
                dbModel.Id = Guid.NewGuid();
                dbModel.NameTransId = NameTransId;
                dbModel.RemarkTransId = RemarkTransId;
                dbModel.CreateBy = Guid.Parse(CurrentUser.UserId);
                dbModel.CreateDate = DateTime.Now;
                dbModel.IsActive = true;
                dbModel.IsDeleted = false;
                baseRepository.Insert(dbModel);
                //update zone country and procince
                InsertZoneProvince(model.Id, cIds);
                //添加規則生成價格
                var tmpZone = baseRepository.GetModel<ExpressZone>(d => d.ExpressId == model.ExpressId && d.IsDeleted == false);
                if (tmpZone != null)
                {
                    var tmpPricelist = baseRepository.GetList<ExpressPrice>().Where(d => d.ZoneId == tmpZone.Id).ToList();
                    if (tmpPricelist.Count() > 0)
                    {
                        foreach (var tmp in tmpPricelist)
                        {
                            ExpressPrice price = new ExpressPrice();
                            price.Id = Guid.NewGuid();
                            price.ZoneId = model.Id;
                            price.WeightFrom = tmp.WeightFrom;
                            price.WeightTo = tmp.WeightTo;
                            price.Price = tmp.Price;
                            price.RuleId = tmp.RuleId;
                            price.CreateBy = Guid.Parse(CurrentUser.UserId);
                            price.CreateDate = DateTime.Now;
                            price.IsActive = true;
                            price.IsDeleted = false;
                            baseRepository.Insert(price);
                        }
                        result.Message = BDMall.Resources.Message.SaveSuccessButMod;
                    }
                    else
                    {
                        result.Message = BDMall.Resources.Message.SaveSuccess;
                    }
                }
                else
                {
                    result.Message = BDMall.Resources.Message.SaveSuccess;
                }
                result.ReturnValue = model.Id;

            }
            UnitOfWork.Submit();
            result.Succeeded = true;

            return result;
        }

        /// <summary>
        /// 更新區域適用省份
        /// </summary>
        /// <param name="zoneId">區域編號</param>
        /// <param name="cIds">適用國家</param>
        private void InsertZoneProvince(Guid zoneId, List<exCounProvince> cIds)
        {
            var oldCountry = baseRepository.GetList<ExpressZoneCountry>().Where(d => d.ZoneId == zoneId).Select(d => d).ToList();
            var oldProvince = baseRepository.GetList<ExpressZoneProvince>().Where(d => d.ZoneId == zoneId).Select(d => d).ToList();
            if (oldProvince != null)
            {
                baseRepository.Delete(oldProvince);
            }
            if (oldCountry != null)
            {
                baseRepository.Delete(oldCountry);
            }
            if (cIds != null)
            {
                foreach (var c in cIds)
                {
                    ExpressZoneCountry country = new ExpressZoneCountry();
                    country.ZoneId = zoneId;
                    country.CountryId = c.country;
                    country.CreateBy = Guid.Parse(CurrentUser.UserId);
                    country.CreateDate = DateTime.Now;
                    country.IsActive = true;
                    country.IsDeleted = false;
                    baseRepository.Insert(country);
                    foreach (var p in c.province)
                    {
                        ExpressZoneProvince province = new ExpressZoneProvince();
                        province.ZoneId = zoneId;
                        province.ProvinceId = Int32.Parse(p);
                        province.CountryId = c.country;
                        province.CreateBy = Guid.Parse(CurrentUser.UserId);
                        province.CreateDate = DateTime.Now;
                        province.IsActive = true;
                        province.IsDeleted = false;
                        baseRepository.Insert(province);
                    }
                }
            }


        }
        public bool StringHtmlContentCheck(ExpressZoneDto model)
        {
            if (model != null)
            {
                if (StringUtil.CheckHasHTMLTag(model.Name))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Code))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(model.Remark))
                {
                    return true;
                }

                if (CheckMultLangListHasHTMLTag(model.Names))
                {
                    return true;
                }
                if (CheckMultLangListHasHTMLTag(model.Remarks))
                {
                    return true;
                }
            }

            return false;
        }

        private List<MutiLanguage> GetMultiLangList(Guid transId)
        {
            try
            {
                var multiLangs = new List<MutiLanguage>();

                multiLangs = _translationRepository.GetMutiLanguage(transId);
                if (!(multiLangs?.Count > 0))
                {
                    multiLangs = GetEmptyMultiLangs();
                }
                return multiLangs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetMultiLangVal(List<MutiLanguage> multiLangs)
        {
            string tranVal = string.Empty;
            if (multiLangs?.Count > 0)
            {
                var multiLang = multiLangs.FirstOrDefault(x => x.Language == CurrentUser.Lang);
                if (multiLang != null)
                {
                    tranVal = multiLang.Desc;
                }
            }
            return tranVal;
        }

        private List<MutiLanguage> GetEmptyMultiLangs()
        {
            if (_EmptyLangs == null)
            {
                var supportLangs = GetSupportLanguage();
                _EmptyLangs = LangUtil.GetMutiLangFromTranslation(null, supportLangs);
            }
            return _EmptyLangs;
        }

        /// <summary>
        /// 根據國家獲取相關省份
        /// </summary>
        /// <param name="countryId">國家ID</param>
        /// <returns></returns>
        public List<CountryZoneView> GetProvinceByCountryZoneForSelect(Guid zoneid, Guid id)
        {
            List<int> selectedProvince = new List<int>();
            List<CountryZoneView> selectedCountryProvince = new List<CountryZoneView>();


            var ids =  baseRepository.GetList<ExpressCountry>(d => d.ExpressId == id && d.IsDeleted == false).Select(d => d.CountryId).ToList();
            List<int> selectedCountry = new List<int>();
            var orderZone = baseRepository.GetList<ExpressZone> (d => d.ExpressId == id && !d.Id.Equals(zoneid) && d.IsDeleted == false).Select(d => d.Id).ToList();
            if (orderZone?.Count() > 0)
            {
                var selectedCountryTemp = baseRepository.GetList<ExpressZoneCountry>(d => orderZone.Contains(d.ZoneId)).Select(d => d.CountryId).ToList();
                foreach (var cid in selectedCountryTemp)
                {
                    var proNum = baseRepository.GetList<Province>(d => d.CountryId == cid && d.IsDeleted == false).Count();
                    var selproNum = baseRepository.GetList<ExpressZoneProvince>(d => d.CountryId == cid && d.ZoneId == zoneid && d.IsDeleted == false).Count();
                    if (proNum == selproNum)
                    {
                        selectedCountry.Add(cid);
                    }
                }
            }
            var dataCountry = baseRepository.GetList<Country>(p => ids.Contains(p.Id) && !selectedCountry.Contains(p.Id)).ToList();
            //var list = dataCountry.Select(d => new KeyValue
            //{
            //    Id = d.Id.ToString(),
            //    Text = NameUtil.GetCountryName(CurrentUser.Language.ToString(), d),
            //}).ToList();

            if (dataCountry.Count() > 0)
            {
                foreach (var country in dataCountry)
                {

                    //var orderZone = _expressZoneRepository.Entities.Where(d => d.ExpressId == id && !d.Id.Equals(zoneid) && d.IsDeleted == false).Select(d => d.Id).ToList();
                    if (orderZone?.Count() > 0)
                    {
                        selectedProvince = baseRepository.GetList<ExpressZoneProvince>(d => d.IsDeleted == false && d.CountryId == country.Id && orderZone.Contains(d.ZoneId)).Select(d => d.ProvinceId).ToList();
                    }
                    var dblist = baseRepository.GetList<Province>(d => d.IsDeleted == false && d.CountryId == country.Id && !selectedProvince.Contains(d.Id)).ToList();
                    var list = AutoMapperExt.MapToList<Province,ProvinceDto>(dblist);
                    foreach (var item in list)
                    {
                        item.Name = NameUtil.GetProviceName(CurrentUser.Lang.ToString(), item);
                    }
                    var keyList = list.Select(d => new KeyValue
                    {
                        Id = d.Id.ToString(),
                        Text = d.Name,
                    }).ToList();
                    var view = new CountryZoneView();
                    view.CountryId = country.Id;
                    view.Province = keyList;
                    selectedCountryProvince.Add(view);
                }
            }

            return selectedCountryProvince;
        }
    }
}
