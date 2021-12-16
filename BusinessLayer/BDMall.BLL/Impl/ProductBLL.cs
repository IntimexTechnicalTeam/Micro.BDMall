using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class ProductBLL : BaseBLL, IProductBLL
    {
        public ProductBLL(IServiceProvider services) : base(services)
        {
        }

        public Dictionary<string, ClickRateSummaryView> GetClickRateView(int topMonthQty, int topWeekQty, int topDayQty)
        {
            var dic = new Dictionary<string, ClickRateSummaryView>();

            var merchId = Guid.Parse(CurrentUser.UserId);
            var isMerch = CurrentUser.LoginType == LoginType.Merchant ? true : false;
            DateTime today = DateTime.Now.Date;
            int thisYear = today.Year;//當前年
            int thisMonth = today.Month;//當前月 

            var baseQuery = (from ps in baseRepository.GetList<ProductClickRateSummry>()
                             join p in baseRepository.GetList<Product>()
                             on new { a1 = ps.ProductCode, a2 = true, a3 = false } equals new { a1 = p.Code, a2 = p.IsActive, a3 = p.IsDeleted }
                             join t in baseRepository.GetList<Translation>() on new { a1 = p.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                             from tt in tc.DefaultIfEmpty()
                             where  ps.IsActive && ps.IsDeleted == false && (!isMerch || (isMerch && ps.MerchantId == merchId))
                             && ps.Year == thisYear && ps.Month == thisMonth
                             select new ClickOrSearchDto
                             {
                                 ProductCode = ps.ProductCode,
                                 Year = ps.Year,
                                 Month = ps.Month,
                                 Day = ps.Day,
                                 ClickQty = ps.ClickCounter,
                                 ProductName = tt,
                             }).ToList();

            ///处理月                   
            var monthClickView = GenClickRateView(baseQuery, topMonthQty, TimePrecisionType.Month);
            dic.Add("chartMonthlyClickRate", monthClickView);

            ///处理周         
            var weekClickView = GenClickRateView(baseQuery, topWeekQty, TimePrecisionType.Week);
            dic.Add("chartWeeklyClickRate", weekClickView);

            ///处理日
            var dayClickView = GenClickRateView(baseQuery, topDayQty, TimePrecisionType.Day);
            dic.Add("chartTodayClickRate", dayClickView);

            return dic;
        }

        public Dictionary<string, ClickRateSummaryView> GetSearchRateView(int topMonthQty, int topWeekQty, int topDayQty)
        {
            var dic = new Dictionary<string, ClickRateSummaryView>();

            var merchId = Guid.Parse(CurrentUser.UserId);
            var isMerch = CurrentUser.LoginType == LoginType.Merchant ? true : false;
            DateTime today = DateTime.Now.Date;
            int thisYear = today.Year;//當前年
            int thisMonth = today.Month;//當前月 

            var baseQuery = (from ps in baseRepository.GetList<ProductClickRateSummry>()
                             join p in baseRepository.GetList<Product>()
                             on new { a1 = ps.ProductCode, a2 = true, a3 = false } equals new { a1 = p.Code, a2 = p.IsActive, a3 = p.IsDeleted }
                             join t in baseRepository.GetList<Translation>() on new { a1 = p.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                             from tt in tc.DefaultIfEmpty()
                             where ps.IsActive && ps.IsDeleted == false && (!isMerch || (isMerch && ps.MerchantId == merchId))
                             && ps.Year == thisYear && ps.Month == thisMonth
                             select new ClickOrSearchDto
                             {
                                 ProductCode = ps.ProductCode,
                                 Year = ps.Year,
                                 Month = ps.Month,
                                 Day = ps.Day,
                                 SearchClickQty = ps.SearchClickCounter,
                                 ProductName = tt,
                             }).ToList();

            var monthSearchView = GenSearchRateView(baseQuery, topMonthQty, TimePrecisionType.Month);
            dic.Add("chartMonthlySearchClickRate", monthSearchView);

            var weekSearchView = GenSearchRateView(baseQuery, topWeekQty, TimePrecisionType.Week);
            dic.Add("chartWeeklySearchClickRate", weekSearchView);

            var daySearchView = GenSearchRateView(baseQuery, topDayQty, TimePrecisionType.Day);
            dic.Add("chartTodaySearchClickRate", daySearchView);

            return dic;
        }

        public PageData<ProductSummary> SearchBackEndProductSummary(ProdSearchCond cond)
        {
            PageData<ProductSummary> result = new PageData<ProductSummary>();
            try
            {
                //result = _productRepository.Search(cond);
                //var currency = CurrencyBLL.GetDefaultCurrency();
                //foreach (var item in result.Data)
                //{
                //    item.Imgs = GetProductImages(item.ProductId);
                //    item.IconRUrl = PathUtil.GetProductIconUrl(item.IconRType, CurrentUser.ComeFrom, CurrentUser.Language);
                //    //item.Currency = currency;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
























        /// <summary>
        /// 整合产品浏览（日，周，月）
        /// </summary>
        /// <param name="baseQuery"></param>
        /// <param name="topQty"></param>
        /// <param name="precisionType"></param>
        /// <returns></returns>
        ClickRateSummaryView GenClickRateView(List<ClickOrSearchDto> baseQuery, int topQty, TimePrecisionType precisionType)
        {
            var queryLst = new List<ClickRateSummary>();
            var rateView = new ClickRateSummaryView();

            string titleName = DateTime.Now.Date.ToString("MMMM", CultureInfo.GetCultureInfo("en-US"));
            if (topQty > 0)
            {
                DateTime today = DateTime.Now.Date;
                int thisDay = today.Day;//當前日

                switch (precisionType)
                {
                    case TimePrecisionType.Week://週匯總數據
                        {
                            #region 週匯總

                            int weekNum = Convert.ToInt32(today.DayOfWeek);//星期數
                            int firstDay = today.AddDays(-weekNum + 1).Day;//周開始日
                            int lastDay = today.AddDays(7 - weekNum).Day;//周結束日
                            int thisWeek = GetWeekNumInMonth(today);//當前周

                            queryLst = baseQuery.Where(x => x.Day >= firstDay && x.Day <= lastDay)
                                .GroupBy(x => new { x.Year, x.Month, x.ProductCode })
                                .Select(d => new ClickRateSummary
                                {
                                    ProductCode = d.Key.ProductCode,
                                    Year = d.Key.Year,
                                    Month = d.Key.Month,
                                    Week = thisWeek,
                                    ClickQty = d.Sum(x => x.ClickQty),
                                    ProductName = d.Select(s => s.ProductName).FirstOrDefault().Value ?? "",
                                })
                                .OrderByDescending(o => o.ClickQty).Take(topQty).ToList();

                            titleName = $"Week {queryLst?.FirstOrDefault()?.Week?.ToString() ?? "" }";
                            #endregion
                        }
                        break;
                    case TimePrecisionType.Day://日匯總數據
                        {
                            #region 日匯總

                            queryLst = baseQuery.Where(x => x.Day == thisDay)
                                .GroupBy(x => new { x.Year, x.Month, x.Day, x.ProductCode })
                                .Select(d => new ClickRateSummary
                                {
                                    ProductCode = d.Key.ProductCode,
                                    Year = d.Key.Year,
                                    Month = d.Key.Month,
                                    ClickQty = d.Sum(x => x.ClickQty),
                                    ProductName = d.Select(s => s.ProductName).FirstOrDefault().Value ?? "",
                                })
                                .OrderByDescending(o => o.ClickQty).Take(topQty).ToList();

                            titleName = DateTime.Now.Date.ToString("MMMM", CultureInfo.GetCultureInfo("en-US")) + "-" + DateTime.Now.Date.Day.ToString().PadLeft(2, '0');

                            #endregion
                        }
                        break;
                    default://默認月匯總數據
                        {
                            #region 月匯總

                            queryLst = baseQuery.GroupBy(x => new { x.Year, x.Month, x.ProductCode })
                                .Select(d => new ClickRateSummary
                                {
                                    ProductCode = d.Key.ProductCode,
                                    Year = d.Key.Year,
                                    Month = d.Key.Month,
                                    ClickQty = d.Sum(x => x.ClickQty),
                                    ProductName = d.Select(s => s.ProductName).FirstOrDefault().Value ?? "",
                                })
                                .OrderByDescending(o => o.ClickQty).Take(topQty).ToList();

                            #endregion
                        }
                        break;
                }

                if (queryLst != null && queryLst.Any())
                {
                    rateView.ClickRateDetailList = queryLst.Where(x => x.ClickQty > 0).Select(s => new ClickRateDetailView { ProductName = s.ProductName, Qty = s.ClickQty }).ToList();
                    rateView.TitleList.Add(titleName);
                }
            }
            return rateView;
        }

        /// <summary>
        /// 整合产品搜索（日，周，月）
        /// </summary>
        /// <param name="baseQuery"></param>
        /// <param name="topQty"></param>
        /// <param name="precisionType"></param>
        /// <returns></returns>
        ClickRateSummaryView GenSearchRateView(List<ClickOrSearchDto> baseQuery, int topQty, TimePrecisionType precisionType)
        {
            var queryLst = new List<ClickRateSummary>();
            var rateView = new ClickRateSummaryView();

            string titleName = DateTime.Now.Date.ToString("MMMM", CultureInfo.GetCultureInfo("en-US"));
            if (topQty > 0)
            {
                DateTime today = DateTime.Now.Date;
                int thisDay = today.Day;//當前日

                switch (precisionType)
                {
                    case TimePrecisionType.Week://週匯總數據
                        {
                            #region 週匯總

                            int weekNum = Convert.ToInt32(today.DayOfWeek);//星期數
                            int firstDay = today.AddDays(-weekNum + 1).Day;//周開始日
                            int lastDay = today.AddDays(7 - weekNum).Day;//周結束日
                            int thisWeek = GetWeekNumInMonth(today);//當前周

                            queryLst = baseQuery.Where(x => x.Day >= firstDay && x.Day <= lastDay)
                                .GroupBy(x => new { x.Year, x.Month, x.ProductCode })
                                .Select(d => new ClickRateSummary
                                {
                                    ProductCode = d.Key.ProductCode,
                                    Year = d.Key.Year,
                                    Month = d.Key.Month,
                                    Week = thisWeek,
                                    SearchClickQty = d.Sum(x => x.SearchClickQty),
                                    ProductName = d.Select(c => c.ProductName).FirstOrDefault().Value ?? ""
                                })
                                .OrderByDescending(o => o.SearchClickQty).Take(topQty).ToList();

                            titleName = $"Week {queryLst?.FirstOrDefault()?.Week?.ToString() ?? "" }";
                            #endregion
                        }
                        break;
                    case TimePrecisionType.Day://日匯總數據
                        {
                            #region 日匯總

                            queryLst = baseQuery.Where(x => x.Day == thisDay)
                                .GroupBy(x => new { x.Year, x.Month, x.Day, x.ProductCode })
                                .Select(d => new ClickRateSummary
                                {
                                    ProductCode = d.Key.ProductCode,
                                    Year = d.Key.Year,
                                    Month = d.Key.Month,
                                    SearchClickQty = d.Sum(x => x.SearchClickQty),
                                    ProductName = d.Select(c => c.ProductName).FirstOrDefault().Value ?? ""
                                })
                                .OrderByDescending(o => o.SearchClickQty).Take(topQty).ToList();

                            titleName = DateTime.Now.Date.ToString("MMMM", CultureInfo.GetCultureInfo("en-US")) + "-" + DateTime.Now.Date.Day.ToString().PadLeft(2, '0');

                            #endregion
                        }
                        break;
                    default://默認月匯總數據
                        {
                            #region 月匯總

                            queryLst = baseQuery.GroupBy(x => new { x.Year, x.Month, x.ProductCode }).Select(d => new ClickRateSummary
                            {
                                ProductCode = d.Key.ProductCode,
                                Year = d.Key.Year,
                                Month = d.Key.Month,
                                SearchClickQty = d.Sum(x => x.SearchClickQty),
                                ProductName = d.Select(c => c.ProductName).FirstOrDefault().Value ?? ""
                            }).OrderByDescending(o => o.ClickQty).Take(topQty).ToList();
                            #endregion
                        }
                        break;
                }

                if (queryLst != null && queryLst.Any())
                {
                    rateView.ClickRateDetailList = queryLst.Where(x => x.SearchClickQty > 0).Select(s => new ClickRateDetailView { ProductName = s.ProductName, Qty = s.SearchClickQty }).ToList();
                    rateView.TitleList.Add(titleName);
                }
            }
            return rateView;
        }

        private int GetWeekNumInMonth(DateTime nowDate)
        {
            int dayInMonth = nowDate.Day;
            //本月第一日
            DateTime firstDay = nowDate.AddDays(1 - nowDate.Day);
            //本月第一日是星期幾
            int weekday = firstDay.DayOfWeek == 0 ? 7 : (int)firstDay.DayOfWeek;
            //本月第一週有幾日
            int firstWeekEndDay = 7 - (weekday - 1);
            //當前日期和第一週之差
            int diffday = dayInMonth - firstWeekEndDay;
            diffday = diffday > 0 ? diffday : 1;
            //當前是第幾周，如果整除7就減一日
            int WeekNumInMonth = ((diffday % 7) == 0 ?
             (diffday / 7 - 1)
             : (diffday / 7)) + 1 + (dayInMonth > firstWeekEndDay ? 1 : 0);
            return WeekNumInMonth;
        }

        
    }
}
