using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
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

        //public ICurrExchangeRateRwpository CurrExchangeRateRwpository { get; set; }
        public ISettingBLL settingBLL { get; set; }

        ICodeMasterRepository _codeMasterRepository;

        public CurrencyBLL(IServiceProvider services) : base(services)
        {

        }

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
                        var rate = baseRepository.GetModel<CurrencyExchangeRate>(d => d.FromCurCode == defCurrencyCode && d.ToCurCode == item.Key && d.IsActive && !d.IsDeleted).Rate;
                        if (rate > 0)
                        {
                            var m = new SimpleCurrency()
                            {
                                Id = item.Id,
                                Code = item.Key,
                                Name = item.Descriptions.SingleOrDefault(d => d.Language == CurrentUser.Lang)?.Desc,
                                ExchangeRate = rate,
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
        //public List<CurrencyExchangeRate> GetCurrExchangeRate(string baseCode)
        //{
        //    var currency = new List<CurrencyExchangeRate>();
        //    var list = GetCurrList();
        //    foreach (var item in list)
        //    {
        //        CurrencyExchangeRate rate = CurrExchangeRateRwpository.Entities.FirstOrDefault(d => d.FromCurCode == baseCode && d.ToCurCode == item.Id && d.IsDeleted == false);
        //        if (rate != null)
        //        {
        //            rate.ToName = GetName(rate.ToCurCode);
        //            currency.Add(rate);
        //        }
        //        else
        //        {
        //            rate = new CurrencyExchangeRate();
        //            rate.FromCurCode = baseCode;
        //            rate.ToCurCode = item.Text;
        //            rate.Rate = 1;
        //            rate.ToName = GetName(rate.ToCurCode);
        //            currency.Add(rate);
        //        }
        //    }
        //    return currency;
        //}
        //public List<KeyValue> GetCurrList()
        //{
        //    var data = _codeMasterRepository.GetCodeMasters(CodeMasterModule.System, CodeMasterFunction.Currency).ToList();
        //    List<KeyValue> list = data.Select(d => new KeyValue { Id = d.Key, Text = d.Key }).ToList();
        //    return list;
        //}

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

        //public SimpleCurrency GetDefaultCurrency()
        //{
        //    try
        //    {
        //        if (DefaultCurrency == null)
        //        {
        //            var code = _codeMasterRepository.GetCodeMaster(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Currency.ToString(), "DefaultCurrency");
        //            if (code == null || string.IsNullOrEmpty(code.Value))
        //            {
        //                throw new DefaultCurrencyException();
        //            }

        //            return GetSimpleCurrency(code.Value);
        //        }
        //        else
        //        {
        //            return DefaultCurrency;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        SaveError(this.GetType().FullName, ClassUtility.GetMethodName(), "", ex.Message);
        //        return null;
        //        //throw ex;
        //    }

        //}

        private string GetName(string code)
        {
            var currency = _codeMasterRepository.GetCodeMaster(CodeMasterModule.System.ToString(), CodeMasterFunction.Currency.ToString(), code);

            return currency.Descriptions.FirstOrDefault(d => d.Language == CurrentUser.Lang).Desc ?? "";
        }

        //public void UpdateRate(CurrencyListView items)
        //{
        //    UnitOfWork.IsUnitSubmit = true;
        //    foreach (var item in items.list)
        //    {
        //        var ent = CurrExchangeRateRwpository.Find(item.Id);
        //        if (ent != null)
        //        {
        //            ent.Rate = item.Rate;
        //            CurrExchangeRateRwpository.Update(ent);
        //        }
        //        else
        //        {
        //            ent = new CurrencyExchangeRate();
        //            ent.Id = Guid.NewGuid();
        //            ent.ToCurCode = item.ToCurCode;
        //            ent.FromCurCode = item.FromCurCode;
        //            ent.Rate = item.Rate;
        //            CurrExchangeRateRwpository.Insert(ent);
        //        }

        //        UpdateCurrencyCache(ent);

        //    }
            
        //    UnitOfWork.Submit();
        //}

        ///// <summary>
        ///// 更新缓存中的CurrencyRate
        ///// </summary>
        ///// <param name="model"></param>
        //private void UpdateCurrencyCache(CurrencyExchangeRate model)
        //{
        //    string key = CacheKey.System.ToString();

        //    var fields = settingBLL.GetSupportLanguages().Select(s => $"{CacheField.Info.ToString()}_{s.Code}").ToArray();
            
        //    foreach (var item in fields)
        //    {
        //        var cacheData = CacheClient.RedisHelper.HGet<SystemInfoDto>(key, item);
        //        if (cacheData != null )
        //        {
        //            if (cacheData.simpleCurrencies.Any(x => x.Code == model.ToCurCode && x.ExchangeRate != model.Rate))
        //            {
        //                cacheData.simpleCurrencies.FirstOrDefault(x => x.Code == model.ToCurCode).ExchangeRate = model.Rate;
        //                CacheClient.RedisHelper.HSet(key, item, cacheData);                     
        //            }
        //        }                
        //    }           
        //}

        //public void SetDefault(string code)
        //{
        //    UnitOfWork.IsUnitSubmit = true;
        //    var newDefault = _codeMasterRepository.GetCodeMaster(CodeMasterModule.Setting.ToString(), CodeMasterFunction.Currency.ToString(), "DefaultCurrency");
        //    if (newDefault != null)
        //    {
        //        newDefault.Value = code;
        //        _codeMasterRepository.Update(newDefault);
        //    }
        //    UnitOfWork.Submit();
        //}

        ///// <summary>
        ///// 獲取指定編號的貨幣信息
        ///// </summary>
        //public CurrencyView GetCurrencyByCode(string code)
        //{
        //    try
        //    {
        //        CurrencyView currencyView = new CurrencyView();
        //        if (!string.IsNullOrEmpty(code))
        //        {
        //            var cmRec = _codeMasterRepository.Entities.FirstOrDefault(x => x.Module == CodeMasterModule.System.ToString() && x.Function == CodeMasterFunction.Currency.ToString() && x.Key == code.Trim() && x.IsActive && !x.IsDeleted);
        //            if (cmRec != null)
        //            {
        //                string defaultCode = GetDefaultCurrencyCode();
        //                var rateRec = CurrExchangeRateRwpository.Entities.FirstOrDefault(x => x.ToCurCode == cmRec.Key && x.FromCurCode == defaultCode && x.IsActive && !x.IsDeleted);
        //                if (rateRec != null)
        //                {
        //                    var descList = TranslationRepository.GetMutiLanguage(cmRec.DescTransId);

        //                    currencyView = new CurrencyView()
        //                    {
        //                        Id = cmRec.Id,
        //                        RateId = rateRec.Id,
        //                        Rate = rateRec.Rate,
        //                        Code = cmRec.Key,
        //                        Description = descList.FirstOrDefault(x => x.Language == CurrentUser.Language)?.Desc ?? string.Empty,
        //                        Remark = cmRec.Remark,
        //                        FromCurCode = rateRec.FromCurCode,
        //                        ToCurCode = rateRec.ToCurCode,
        //                        Descriptions = descList,
        //                    };
        //                }
        //            }
        //        }
        //        else
        //        {
        //            currencyView.Descriptions = TranslationRepository.GetMutiLanguage(Guid.Empty);
        //        }
        //        return currencyView;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        ///// <summary>
        ///// 搜尋貨幣信息列表
        ///// </summary>
        //public List<CurrencyView> SearchCurrencyList(CurrencyPageInfo pageInfo)
        //{
        //    try
        //    {
        //        var currencyList = new List<CurrencyView>();
        //        if (pageInfo != null && pageInfo.Condition != null)
        //        {
        //            var cond = pageInfo.Condition;

        //            var cmList = GetCodeMasterList(CodeMasterModule.System, CodeMasterFunction.Currency);
        //            if (cmList?.Count > 0)
        //            {
        //                if (!string.IsNullOrEmpty(cond.Code))
        //                {
        //                    cmList = cmList.Where(x => x.Key.Contains(cond.Code)).ToList();
        //                }
        //                if (cmList?.Count > 0)
        //                {
        //                    string defaultCode = GetDefaultCurrencyCode();

        //                    foreach (var cmRec in cmList)
        //                    {
        //                        var rateRec = CurrExchangeRateRwpository.Entities.FirstOrDefault(x => x.ToCurCode == cmRec.Key && x.FromCurCode == defaultCode && x.IsActive && !x.IsDeleted);
        //                        if (rateRec != null)
        //                        {
        //                            var descList = TranslationRepository.GetMutiLanguage(cmRec.DescTransId);

        //                            var currencyVw = new CurrencyView()
        //                            {
        //                                Id = cmRec.Id,
        //                                RateId = rateRec.Id,
        //                                Rate = rateRec.Rate,
        //                                Code = cmRec.Key,
        //                                Description = descList.FirstOrDefault(x => x.Language == CurrentUser.Language)?.Desc ?? string.Empty,
        //                                Remark = cmRec.Remark,
        //                                FromCurCode = rateRec.FromCurCode,
        //                                ToCurCode = rateRec.ToCurCode,
        //                                Descriptions = descList,
        //                                CreateDateStr = cmRec.CreateDate.ToString(BDMall.Runtime.Setting.DefaultDateTimeFormat),
        //                                UpdateDateStr = cmRec.UpdateDate.Value.ToString(BDMall.Runtime.Setting.DefaultDateTimeFormat),
        //                            };
        //                            currencyList.Add(currencyVw);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return currencyList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        ///// <summary>
        ///// 新增貨幣
        ///// </summary>
        //public SystemResult InsertCurrency(CurrencyView currency)
        //{
        //    var sysRslt = new SystemResult();
        //    try
        //    {
        //        if (currency != null && !string.IsNullOrEmpty(currency.Code))
        //        {
        //            string currencyCode = currency.Code.ToUpper();
        //            var cmRec = GetCodeMasterRecord(CodeMasterModule.System, CodeMasterFunction.Currency, currencyCode);
        //            if (cmRec == null)
        //            {
        //                UnitOfWork.IsUnitSubmit = true;

        //                cmRec = new CodeMaster();
        //                cmRec.Key = currencyCode;
        //                cmRec.Value = currencyCode;
        //                cmRec.DescTransId = TranslationRepository.InsertMutiLanguage(currency.Descriptions, TranslationType.Currency);
        //                cmRec.Module = CodeMasterModule.System.ToString();
        //                cmRec.Function = CodeMasterFunction.Currency.ToString();
        //                cmRec.Remark = currency.Remark;
        //                _codeMasterRepository.Insert(cmRec);

        //                var rateRec = CurrExchangeRateRwpository.Entities.FirstOrDefault(x => x.ToCurCode == currencyCode && x.IsActive && !x.IsDeleted);
        //                if (rateRec == null)
        //                {
        //                    string defaultCurrencyCode = GetDefaultCurrencyCode();
        //                    rateRec = new CurrencyExchangeRate()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        FromCurCode = defaultCurrencyCode,
        //                        ToCurCode = currencyCode,
        //                        Rate = 0,
        //                    };
        //                    CurrExchangeRateRwpository.Insert(rateRec);

        //                    UnitOfWork.Submit();

        //                    #region CodeMaster 緩存更新

        //                    string cacheKey = GetCodeMasterCacheKey(cmRec);
        //                    CacheManager.Insert(cacheKey, cmRec);

        //                    cmRec.Key = "";
        //                    cacheKey = GetCodeMasterCacheKey(cmRec);
        //                    CacheManager.NoticUpdate(cacheKey);

        //                    #endregion

        //                    sysRslt.Succeeded = true;
        //                }
        //            }
        //            else
        //            {
        //                sysRslt.Message = BDMall.Resources.Label.CurrencyCode + BDMall.Resources.Message.HasExist;
        //                sysRslt.Succeeded = false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sysRslt.Succeeded = false;
        //        sysRslt.Message = ex.Message;
        //    }
        //    return sysRslt;
        //}
        ///// <summary>
        ///// 更新貨幣
        ///// </summary>
        //public SystemResult UpdateCurrency(CurrencyView currency)
        //{
        //    var sysRslt = new SystemResult();
        //    try
        //    {
        //        if (currency != null && !string.IsNullOrEmpty(currency.Code))
        //        {
        //            string currencyCode = currency.Code;

        //            var cmRec = GetCodeMasterRecord(CodeMasterModule.System, CodeMasterFunction.Currency, currencyCode);
        //            if (cmRec != null)
        //            {
        //                UnitOfWork.IsUnitSubmit = true;

        //                TranslationRepository.UpdateMutiLanguage(cmRec.DescTransId, currency.Descriptions, TranslationType.Currency);
        //                cmRec.Remark = currency.Remark;

        //                _codeMasterRepository.Update(cmRec);

        //                UnitOfWork.Submit();

        //                #region CodeMaster 緩存更新

        //                string cacheKey = GetCodeMasterCacheKey(cmRec);

        //                CacheManager.NoticUpdate(cacheKey);


        //                #region  for gen new cachekey
        //                string key = cmRec.Key;
        //                cmRec.Key = "";
        //                cacheKey = GetCodeMasterCacheKey(cmRec);
        //                cmRec.Key = key;
        //                #endregion

        //                CacheManager.NoticUpdate(cacheKey);

        //                #endregion

        //                sysRslt.Succeeded = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sysRslt.Succeeded = false;
        //        sysRslt.Message = ex.Message;
        //    }
        //    return sysRslt;
        //}
        ///// <summary>
        ///// 刪除貨幣
        ///// </summary>
        //public SystemResult DeleteCurrency(CurrencyView currency)
        //{
        //    var sysRslt = new SystemResult();
        //    try
        //    {
        //        if (currency != null && !string.IsNullOrEmpty(currency.Code))
        //        {
        //            string currencyCode = currency.Code;
        //            string defaultCurrencyCode = GetDefaultCurrencyCode();

        //            if (currencyCode != defaultCurrencyCode)
        //            {
        //                UnitOfWork.IsUnitSubmit = true;

        //                var cmRec = GetCodeMasterRecord(CodeMasterModule.System, CodeMasterFunction.Currency, currencyCode);
        //                if (cmRec != null)
        //                {
        //                    cmRec.IsActive = false;
        //                    cmRec.IsDeleted = true;
        //                    _codeMasterRepository.Update(cmRec);
        //                }

        //                var rateRec = CurrExchangeRateRwpository.Entities.FirstOrDefault(x => x.ToCurCode == currencyCode && x.FromCurCode == defaultCurrencyCode && x.IsActive && !x.IsDeleted);
        //                if (rateRec != null)
        //                {
        //                    rateRec.IsActive = false;
        //                    rateRec.IsDeleted = true;
        //                    CurrExchangeRateRwpository.Update(rateRec);
        //                }

        //                UnitOfWork.Submit();

        //                #region CodeMaster 緩存更新

        //                string cacheKey = GetCodeMasterCacheKey(cmRec);

        //                CacheManager.NoticUpdate(cacheKey);


        //                #region  for gen new cachekey
        //                string key = cmRec.Key;
        //                cmRec.Key = "";
        //                cacheKey = GetCodeMasterCacheKey(cmRec);
        //                cmRec.Key = key;
        //                #endregion

        //                CacheManager.NoticUpdate(cacheKey);

        //                #endregion

        //                sysRslt.Succeeded = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sysRslt.Succeeded = false;
        //        sysRslt.Message = ex.Message;
        //    }
        //    return sysRslt;
        //}

        ///// <summary>
        ///// 刪除多個貨幣資料
        ///// </summary>
        ///// <param name="recIdList"></param>
        ///// <returns></returns>
        //public SystemResult DeleteCurrencyList(string recIdList)
        //{
        //    var sysRslt = new SystemResult();
        //    try
        //    {
        //        string[] codeList = recIdList.Split(',');

        //        UnitOfWork.IsUnitSubmit = true;

        //        foreach (var code in codeList)
        //        {
        //            if (!string.IsNullOrEmpty(code))
        //            {
        //                string currencyCode = code.Trim();
        //                string defaultCurrencyCode = GetDefaultCurrencyCode();

        //                if (currencyCode != defaultCurrencyCode)
        //                {
        //                    var cmRec = GetCodeMasterRecord(CodeMasterModule.System, CodeMasterFunction.Currency, currencyCode);
        //                    if (cmRec != null)
        //                    {
        //                        cmRec.IsActive = false;
        //                        cmRec.IsDeleted = true;
        //                        _codeMasterRepository.Update(cmRec);
        //                    }

        //                    var rateRec = CurrExchangeRateRwpository.Entities.FirstOrDefault(x => x.ToCurCode == currencyCode && x.FromCurCode == defaultCurrencyCode && x.IsActive && !x.IsDeleted);
        //                    if (rateRec != null)
        //                    {
        //                        rateRec.IsActive = false;
        //                        rateRec.IsDeleted = true;
        //                        CurrExchangeRateRwpository.Update(rateRec);
        //                    }

        //                    UnitOfWork.Submit();

        //                    #region CodeMaster 緩存更新

        //                    string cacheKey = GetCodeMasterCacheKey(cmRec);

        //                    CacheManager.NoticUpdate(cacheKey);


        //                    #region  for gen new cachekey
        //                    string key = cmRec.Key;
        //                    cmRec.Key = "";
        //                    cacheKey = GetCodeMasterCacheKey(cmRec);
        //                    cmRec.Key = key;
        //                    #endregion

        //                    CacheManager.NoticUpdate(cacheKey);

        //                    #endregion

        //                    sysRslt.Succeeded = true;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sysRslt.Succeeded = false;
        //        sysRslt.Message = ex.Message;
        //    }
        //    return sysRslt;
        //}

        //private CodeMaster GetCodeMasterRecord(CodeMasterModule module, CodeMasterFunction function, string currencyCode)
        //{
        //    //var cmRec = _codeMasterRepository.Entities.FirstOrDefault(x => x.Module == module.ToString() && x.Function == function.ToString() && x.Key == currencyCode.Trim() && x.IsActive && !x.IsDeleted);
        //    var cmRec = _codeMasterRepository.GetCodeMaster(module.ToString(), function.ToString(), currencyCode);
        //    return cmRec;
        //}

        //private List<CodeMaster> GetCodeMasterList(CodeMasterModule module, CodeMasterFunction function)
        //{
        //    //var cmList = _codeMasterRepository.Entities.Where(x => x.Module == CodeMasterModule.System.ToString() && x.Function == CodeMasterFunction.Currency.ToString() && x.IsActive && !x.IsDeleted).ToList();
        //    var cmList = _codeMasterRepository.GetCodeMasters(module, function);
        //    return cmList;
        //}

        //private string GetCodeMasterCacheKey(CodeMaster codeMaster)
        //{
        //    string cacheKey = string.Format(CodeMasterRepository.CacheKeyFormat, codeMaster.ClientId, codeMaster.Module, codeMaster.Function, codeMaster.Key, CurrentUser.Lang);
        //    return cacheKey;
        //}
    }
}
