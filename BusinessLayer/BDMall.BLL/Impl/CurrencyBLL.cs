using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Runtime;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class CurrencyBLL : BaseBLL, ICurrencyBLL
    {
        private static IDictionary<string, List<SimpleCurrency>> AllCurrencies;
        private static SimpleCurrency DefaultCurrency;
        ITranslationRepository translationRepository;
        //public ICurrExchangeRateRwpository CurrExchangeRateRwpository { get; set; }
        ISettingBLL settingBLL;

        ICodeMasterRepository _codeMasterRepository;

        public CurrencyBLL(IServiceProvider services) : base(services)
        {
            settingBLL = Services.Resolve<ISettingBLL>();
            _codeMasterRepository = Services.Resolve<ICodeMasterRepository>();
            translationRepository = Services.Resolve<ITranslationRepository>();
        }
        //public Currency GetCurrency(Guid id)
        //{
        //    //
        //    throw new NotImplementedException();
        //}

        public SimpleCurrency GetSimpleCurrency(string code)
        {
            var sc = GetCurrencys().SingleOrDefault(d => d.Code == code);
            if (sc == null)
            {
                var model = _codeMasterRepository.GetCodeMaster(CodeMasterModule.System.ToString(), CodeMasterFunction.Currency.ToString(), code);
                if (model != null)
                {
                    var defCurrencyCode = GetDefaultCurrencyCode();
                    decimal rate = 1;
                    if (defCurrencyCode != model.Key)
                    {
                        rate = baseRepository.GetModel<CurrencyExchangeRate>(d => d.FromCurCode == defCurrencyCode && d.ToCurCode == model.Key && d.IsActive && !d.IsDeleted).Rate;
                    }
                    return new SimpleCurrency()
                    {
                        Id = model.Id,
                        Code = model.Key,
                        Name = model.Descriptions.SingleOrDefault(d => d.Language == CurrentUser.Lang).Desc,
                        ExchangeRate = rate
                    };
                }
                return null;
            }
            else
            {
                return sc;
            }

        }
        public List<SimpleCurrency> GetCurrencys()
        {
            string lang = CurrentUser.Lang.ToString();
            List<SimpleCurrency> data = null;
            if (AllCurrencies == null)
            {
                AllCurrencies = new Dictionary<string, List<SimpleCurrency>>();
            }
            else
            {
                if (AllCurrencies.ContainsKey(lang))
                {
                    data = AllCurrencies[lang];
                }

            }
            if (data == null || data.Count == 0)
            {
                data = new List<SimpleCurrency>();
                var defCurrencyCode = GetDefaultCurrencyCode();
                var models = _codeMasterRepository.GetCodeMasters(CodeMasterModule.System.ToString(), CodeMasterFunction.Currency.ToString());
                if (models != null)
                {
                    foreach (var item in models)
                    {
                        var rate = baseRepository.GetModel<CurrencyExchangeRate>(d => d.FromCurCode == defCurrencyCode && d.ToCurCode == item.Key && d.IsActive && !d.IsDeleted);
                        if (rate !=null )
                        {
                            var m = new SimpleCurrency()
                            {
                                Id = item.Id,
                                Code = item.Key,
                                Name = item.Descriptions.FirstOrDefault(d => d.Language == CurrentUser.Lang)?.Desc,
                                ExchangeRate = rate.Rate ,
                            };
                            data.Add(m);
                        }
                       
                    }
                }
                AllCurrencies.Add(lang, data);
            }
            return data;
        }
        //#endregion
        public List<CurrencyExchangeRateDto> GetCurrExchangeRate(string baseCode)
        {
            var currency = new List<CurrencyExchangeRateDto>();
            var list = GetCurrList();
            foreach (var item in list)
            {
                var rate = baseRepository.GetList<CurrencyExchangeRate>().FirstOrDefault(d => d.FromCurCode == baseCode && d.ToCurCode == item.Id && d.IsDeleted == false);
                if (rate != null)
                {
                    var rateDto = AutoMapperExt.MapTo<CurrencyExchangeRateDto>(rate);

                    rateDto.ToName = GetName(rate.ToCurCode);
                    currency.Add(rateDto);
                }
                else
                {
                    var rateDto = new CurrencyExchangeRateDto();
                    rateDto.FromCurCode = baseCode;
                    rateDto.ToCurCode = item.Text;
                    rateDto.Rate = 1;
                    rateDto.ToName = GetName(rate.ToCurCode);
                    currency.Add(rateDto);
                }
            }
            return currency;
        }
        public List<KeyValue> GetCurrList()
        {
            var data = _codeMasterRepository.GetCodeMasters(CodeMasterModule.System, CodeMasterFunction.Currency).ToList();
            List<KeyValue> list = data.Select(d => new KeyValue { Id = d.Key, Text = d.Key }).ToList();
            return list;
        }

        public string GetDefaultCurrencyCode()
        {

            var code = _codeMasterRepository.GetCodeMaster(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Currency.ToString(), "DefaultCurrency");

            //FileLogger.Debug("GetDefaultCurrencyCode db conn=:" + UnitOfWork.DataContext.Database.Connection.ConnectionString);

            if (code == null || string.IsNullOrEmpty(code.Value))
            {
                throw new BLException();
            }

            return code.Value;


        }

        public SimpleCurrency GetDefaultCurrency()
        {

            if (DefaultCurrency == null)
            {
                var code = _codeMasterRepository.GetCodeMaster(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Currency.ToString(), "DefaultCurrency");
                if (code == null || string.IsNullOrEmpty(code.Value))
                {
                    throw new BLException();
                }

                return GetSimpleCurrency(code.Value);
            }
            else
            {
                return DefaultCurrency;
            }
        }

        private string GetName(string code)
        {
            var currency = _codeMasterRepository.GetCodeMaster(CodeMasterModule.System.ToString(), CodeMasterFunction.Currency.ToString(), code);

            return currency.Descriptions.FirstOrDefault(d => d.Language == CurrentUser.Lang).Desc ?? "";
        }

        public void UpdateRate(CurrencyListView items)
        {
            UnitOfWork.IsUnitSubmit = true;
            var cachList = new List<CurrencyExchangeRate>();
            foreach (var item in items.list)
            {
                var ent = baseRepository.GetList<CurrencyExchangeRate>().FirstOrDefault(p => p.Id == item.Id);
                if (ent != null)
                {
                    ent.Rate = item.Rate;
                    baseRepository.Update(ent);
                }
                else
                {
                    ent = new CurrencyExchangeRate();
                    ent.Id = Guid.NewGuid();
                    ent.ToCurCode = item.ToCurCode;
                    ent.FromCurCode = item.FromCurCode;
                    ent.Rate = item.Rate;
                    baseRepository.Insert(ent);
                }
                cachList.Add(ent);
            }

            UnitOfWork.Submit();

            foreach (var item in cachList)
            {
                UpdateCurrencyCache(item);
            }
        }

        /// <summary>
        /// 更新缓存中的CurrencyRate
        /// </summary>
        /// <param name="model"></param>
        private void UpdateCurrencyCache(CurrencyExchangeRate model)
        {
            string key = CacheKey.System.ToString();

            var fields = settingBLL.GetSupportLanguages().Select(s => $"{CacheField.Info.ToString()}_{s.Code}").ToArray();

            foreach (var item in fields)
            {
                var cacheData = RedisHelper.HGet<SystemInfoDto>(key, item);
                if (cacheData != null)
                {
                    if (cacheData.simpleCurrencies.Any(x => x.Code == model.ToCurCode && x.ExchangeRate != model.Rate))
                    {
                        cacheData.simpleCurrencies.FirstOrDefault(x => x.Code == model.ToCurCode).ExchangeRate = model.Rate;
                        RedisHelper.HSet(key, item, cacheData);
                    }
                }
            }
        }

        public void SetDefault(string code)
        {
            UnitOfWork.IsUnitSubmit = true;
            var newDefault = baseRepository.GetModel<CodeMaster>(p => p.Module == CodeMasterModule.Setting.ToString() && p.Function == CodeMasterModule.Setting.ToString() && p.Key == "DefaultCurrency");
            if (newDefault != null)
            {
                newDefault.Value = code;
                baseRepository.Update(newDefault);
            }
            UnitOfWork.Submit();
        }

        /// <summary>
        /// 獲取指定編號的貨幣信息
        /// </summary>
        public CurrencyView GetCurrencyByCode(string code)
        {

            CurrencyView currencyView = new CurrencyView();
            if (!string.IsNullOrEmpty(code))
            {
                var cmRec = baseRepository.GetList<CodeMaster>().FirstOrDefault(x => x.Module == CodeMasterModule.System.ToString() && x.Function == CodeMasterFunction.Currency.ToString() && x.Key == code.Trim() && x.IsActive && !x.IsDeleted);
                if (cmRec != null)
                {
                    string defaultCode = GetDefaultCurrencyCode();
                    var rateRec = baseRepository.GetList<CurrencyExchangeRate>().FirstOrDefault(x => x.ToCurCode == cmRec.Key && x.FromCurCode == defaultCode && x.IsActive && !x.IsDeleted);
                    if (rateRec != null)
                    {
                        var descList = translationRepository.GetMutiLanguage(cmRec.DescTransId);

                        currencyView = new CurrencyView()
                        {
                            Id = cmRec.Id,
                            RateId = rateRec.Id,
                            Rate = rateRec.Rate,
                            Code = cmRec.Key,
                            Description = descList.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc ?? string.Empty,
                            Remark = cmRec.Remark,
                            FromCurCode = rateRec.FromCurCode,
                            ToCurCode = rateRec.ToCurCode,
                            Descriptions = descList,
                        };
                    }
                }
            }
            else
            {
                currencyView.Descriptions = translationRepository.GetMutiLanguage(Guid.Empty);
            }
            return currencyView;

        }
        /// <summary>
        /// 搜尋貨幣信息列表
        /// </summary>
        public List<CurrencyView> SearchCurrencyList(CurrencyPageInfo pageInfo)
        {
            try
            {
                var currencyList = new List<CurrencyView>();
                if (pageInfo != null && pageInfo.Condition != null)
                {
                    var cond = pageInfo.Condition;

                    var cmList = GetCodeMasterList(CodeMasterModule.System, CodeMasterFunction.Currency);
                    if (cmList?.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(cond.Code))
                        {
                            cmList = cmList.Where(x => x.Key.Contains(cond.Code)).ToList();
                        }
                        if (cmList?.Count > 0)
                        {
                            string defaultCode = GetDefaultCurrencyCode();

                            foreach (var cmRec in cmList)
                            {
                                var rateRec = baseRepository.GetList<CurrencyExchangeRate>().FirstOrDefault(x => x.ToCurCode == cmRec.Key && x.FromCurCode == defaultCode && x.IsActive && !x.IsDeleted);
                                if (rateRec != null)
                                {
                                    var descList = translationRepository.GetMutiLanguage(cmRec.DescTransId);

                                    var currencyVw = new CurrencyView()
                                    {
                                        Id = cmRec.Id,
                                        RateId = rateRec.Id,
                                        Rate = rateRec.Rate,
                                        Code = cmRec.Key,
                                        Description = descList.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc ?? string.Empty,
                                        Remark = cmRec.Remark,
                                        FromCurCode = rateRec.FromCurCode,
                                        ToCurCode = rateRec.ToCurCode,
                                        Descriptions = descList,
                                        CreateDateStr = cmRec.CreateDate.ToString(Setting.DefaultDateTimeFormat),
                                        UpdateDateStr = cmRec.UpdateDate.Value.ToString(Setting.DefaultDateTimeFormat),
                                    };
                                    currencyList.Add(currencyVw);
                                }
                            }
                        }
                    }
                }
                return currencyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 新增貨幣
        /// </summary>
        public SystemResult InsertCurrency(CurrencyView currency)
        {
            var sysRslt = new SystemResult();
            string currencyCode = currency.Code.ToUpper();
            if (currency != null && !string.IsNullOrEmpty(currency.Code))
            {
                var cmRec = GetCodeMasterRecord(CodeMasterModule.System, CodeMasterFunction.Currency, currencyCode);

                UnitOfWork.IsUnitSubmit = true;
                if (cmRec == null)
                {
                    var dbCmRec = new CodeMaster();
                    dbCmRec.Key = currencyCode;
                    dbCmRec.Value = currencyCode;
                    dbCmRec.DescTransId = translationRepository.InsertMutiLanguage(currency.Descriptions, TranslationType.Currency);
                    dbCmRec.Module = CodeMasterModule.System.ToString();
                    dbCmRec.Function = CodeMasterFunction.Currency.ToString();
                    dbCmRec.Remark = currency.Remark ?? "";
                    dbCmRec.CreateDate = DateTime.Now;
                    dbCmRec.CreateBy = Guid.Parse(CurrentUser.UserId);
                    dbCmRec.UpdateDate = DateTime.Now;
                    dbCmRec.UpdateBy = Guid.Parse(CurrentUser.UserId);
                    dbCmRec.IsActive = true;
                    baseRepository.Insert(dbCmRec);
                }
                var rateRec = baseRepository.GetList<CurrencyExchangeRate>().FirstOrDefault(x => x.ToCurCode == currencyCode && x.IsActive && !x.IsDeleted);
                if (rateRec == null)
                {

                    string defaultCurrencyCode = GetDefaultCurrencyCode();
                    rateRec = new CurrencyExchangeRate()
                    {
                        Id = Guid.NewGuid(),
                        FromCurCode = defaultCurrencyCode,
                        ToCurCode = currencyCode,
                        Rate = 0,
                        CreateDate = DateTime.Now,
                        CreateBy = Guid.Parse(CurrentUser.UserId),
                        UpdateDate = DateTime.Now,
                        UpdateBy = Guid.Parse(CurrentUser.UserId),
                        IsActive = true,
                    };
                    baseRepository.Insert(rateRec);

                }
                else
                {
                    sysRslt.Message = BDMall.Resources.Label.CurrencyCode + BDMall.Resources.Message.HasExist;
                    sysRslt.Succeeded = false;
                }
            }


            UnitOfWork.Submit();
            sysRslt.Succeeded = true;
            return sysRslt;
        }
        /// <summary>
        /// 更新貨幣
        /// </summary>
        public SystemResult UpdateCurrency(CurrencyView currency)
        {
            var sysRslt = new SystemResult();
            if (currency != null && !string.IsNullOrEmpty(currency.Code))
            {
                string currencyCode = currency.Code;

                var cmRec = GetCodeMasterRecord(CodeMasterModule.System, CodeMasterFunction.Currency, currencyCode);
                if (cmRec != null)
                {
                    UnitOfWork.IsUnitSubmit = true;

                    translationRepository.UpdateMutiLanguage(cmRec.DescTransId, currency.Descriptions, TranslationType.Currency);
                    cmRec.Remark = currency.Remark;

                    baseRepository.Update(cmRec);

                    UnitOfWork.Submit();

                    sysRslt.Succeeded = true;
                }
            }
            return sysRslt;
        }
        /// <summary>
        /// 刪除貨幣
        /// </summary>
        public SystemResult DeleteCurrency(CurrencyView currency)
        {
            var sysRslt = new SystemResult();
            if (currency != null && !string.IsNullOrEmpty(currency.Code))
            {
                string currencyCode = currency.Code;
                string defaultCurrencyCode = GetDefaultCurrencyCode();

                if (currencyCode != defaultCurrencyCode)
                {
                    UnitOfWork.IsUnitSubmit = true;

                    var cmRec = GetCodeMasterRecord(CodeMasterModule.System, CodeMasterFunction.Currency, currencyCode);
                    if (cmRec != null)
                    {
                        cmRec.IsActive = false;
                        cmRec.IsDeleted = true;
                        cmRec.UpdateDate = DateTime.Now;
                        cmRec.UpdateBy = Guid.Parse(CurrentUser.UserId);
                        baseRepository.Update(cmRec);
                    }

                    var rateRec = baseRepository.GetList<CurrencyExchangeRate>().FirstOrDefault(x => x.ToCurCode == currencyCode && x.FromCurCode == defaultCurrencyCode && x.IsActive && !x.IsDeleted);
                    if (rateRec != null)
                    {
                        rateRec.IsActive = false;
                        rateRec.IsDeleted = true;
                        rateRec.UpdateDate = DateTime.Now;
                        rateRec.UpdateBy = Guid.Parse(CurrentUser.UserId);
                        baseRepository.Update(rateRec);
                    }

                    UnitOfWork.Submit();

                    sysRslt.Succeeded = true;
                }
            }
            return sysRslt;
        }

        /// <summary>
        /// 刪除多個貨幣資料
        /// </summary>
        /// <param name="recIdList"></param>
        /// <returns></returns>
        public SystemResult DeleteCurrencyList(string recIdList)
        {
            var sysRslt = new SystemResult();
            string[] codeList = recIdList.Split(',');

            UnitOfWork.IsUnitSubmit = true;

            foreach (var code in codeList)
            {
                if (!string.IsNullOrEmpty(code))
                {
                    string currencyCode = code.Trim();
                    string defaultCurrencyCode = GetDefaultCurrencyCode();

                    if (currencyCode != defaultCurrencyCode)
                    {
                        var cmRec = GetCodeMasterRecord(CodeMasterModule.System, CodeMasterFunction.Currency, currencyCode);
                        if (cmRec != null)
                        {
                            cmRec.IsActive = false;
                            cmRec.IsDeleted = true;
                            cmRec.UpdateDate = DateTime.Now;
                            cmRec.UpdateBy = Guid.Parse(CurrentUser.UserId);
                            baseRepository.Update(cmRec);
                        }

                        var rateRec = baseRepository.GetList<CurrencyExchangeRate>().FirstOrDefault(x => x.ToCurCode == currencyCode && x.FromCurCode == defaultCurrencyCode && x.IsActive && !x.IsDeleted);
                        if (rateRec != null)
                        {
                            rateRec.IsActive = false;
                            rateRec.IsDeleted = true;
                            rateRec.UpdateDate = DateTime.Now;
                            rateRec.UpdateBy = Guid.Parse(CurrentUser.UserId);
                            baseRepository.Update(rateRec);
                        }

                        UnitOfWork.Submit();
                        sysRslt.Succeeded = true;
                    }
                }
            }
            return sysRslt;
        }

        private CodeMaster GetCodeMasterRecord(CodeMasterModule module, CodeMasterFunction function, string currencyCode)
        {
            var cmRec = _codeMasterRepository.GetCodeMaster(module.ToString(), function.ToString(), currencyCode);

            var dbCMRec = AutoMapperExt.MapTo<CodeMaster>(cmRec);
            return dbCMRec;
        }

        private List<CodeMaster> GetCodeMasterList(CodeMasterModule module, CodeMasterFunction function)
        {
            var cmList = _codeMasterRepository.GetCodeMasters(module, function);
            var dbCMRecs = AutoMapperExt.MapToList<CodeMasterDto, CodeMaster>(cmList);
            return dbCMRecs;
        }

        //private string GetCodeMasterCacheKey(CodeMasterDto codeMaster)
        //{
        //    string cacheKey = string.Format(CodeMasterRepository.CacheKeyFormat, codeMaster.ClientId, codeMaster.Module, codeMaster.Function, codeMaster.Key, CurrentUser.Lang);
        //    return cacheKey;
        //}
    }
}
