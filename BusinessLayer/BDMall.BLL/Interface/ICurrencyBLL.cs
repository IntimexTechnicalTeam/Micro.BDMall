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
    public interface ICurrencyBLL :IDependency
    {
      
        //CurrencyDto GetCurrency(Guid id);
        SimpleCurrency GetSimpleCurrency(string code);

        List<SimpleCurrency> GetCurrencys();

        List<CurrencyExchangeRate> GetCurrExchangeRate(string baseCode);

        string GetDefaultCurrencyCode();

        //SimpleCurrency GetDefaultCurrency();

        /// <summary>
        /// 獲取貨幣列表
        /// </summary>
        /// <returns></returns>
        List<KeyValue> GetCurrList();

        /// <summary>
        /// 更新匯率
        /// </summary>
        /// <param name="list"></param>
        void UpdateRate(CurrencyListView items);

        void SetDefault(string code);

        /// <summary>
        /// 獲取指定編號的貨幣信息
        /// </summary>
        CurrencyView GetCurrencyByCode(string code);
        /// <summary>
        /// 搜尋貨幣信息列表
        /// </summary>
        ///List<CurrencyView> SearchCurrencyList(CurrencyPageInfo pageInfo);
        /// <summary>
        /// 新增貨幣
        /// </summary>
        SystemResult InsertCurrency(CurrencyView currency);
        /// <summary>
        /// 更新貨幣
        /// </summary>
        SystemResult UpdateCurrency(CurrencyView currency);
        /// <summary>
        /// 刪除貨幣
        /// </summary>
        SystemResult DeleteCurrency(CurrencyView currency);
        /// <summary>
        /// 刪除貨幣列表
        /// </summary>
        SystemResult DeleteCurrencyList(string recIdList);
    }
}
