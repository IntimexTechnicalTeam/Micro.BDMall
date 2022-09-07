using System.Globalization;

namespace BDMall.BLL
{
    public class SalesReportBLL : BaseBLL, ISalesReportBLL
    {
        public SalesReportBLL(IServiceProvider services) : base(services)
        {
        }

        public Dictionary<string, HotSalesSummaryView> GetHotSalesProductList(int topMonthQty, int topWeekQty, int topDayQty, SortType sortType)
        {
            var dic = new Dictionary<string, HotSalesSummaryView>();

            var merchId = CurrentUser.MerchantId;
            var isMerch = CurrentUser.LoginType == LoginType.Merchant ? true : false;

            DateTime today = DateTime.Now.Date;
            int thisYear = today.Year;//當前年
            int thisMonth = today.Month;//當前月 

            var baseQuery = (from ps in baseRepository.GetList<ProductSalesSummry>()
                             join sku in baseRepository.GetList<ProductSku>()
                             on ps.Sku equals sku.Id
                             join p in baseRepository.GetList<Product>()
                             on sku.ProductCode equals p.Code
                             where  ps.IsActive && !ps.IsDeleted && (!isMerch || (isMerch && ps.MerchantId == merchId)) && sku.IsActive && !sku.IsDeleted
                             && p.Status == ProductStatus.OnSale && !p.IsDeleted && (!isMerch || (isMerch && p.MerchantId == merchId))
                             && ps.Year == thisYear && ps.Month == thisMonth
                             select new HotSalesSummary
                             {
                                 Year = ps.Year,
                                 Month = ps.Month,
                                 Day = ps.Day,
                                 TotalSalesQty = ps.Qty,
                                 Sku = ps.Sku,
                             }).ToList();

            ///处理月                   
            var monthSaleView = GenHotSaleView(baseQuery, topMonthQty, TimePrecisionType.Month, SortType.DESC);
            dic.Add("chartMonthlyHotSales", monthSaleView);

            ///处理周         
            var weekSaleView = GenHotSaleView(baseQuery, topWeekQty, TimePrecisionType.Week, SortType.DESC);
            dic.Add("chartWeeklyHotSales", weekSaleView);

            ///处理日
            var daySaleView = GenHotSaleView(baseQuery, topDayQty, TimePrecisionType.Day, SortType.DESC);
            dic.Add("chartTodayHotSales", daySaleView);

            return dic;
        }

        public Dictionary<string, List<OrderShowCaseSummary>> GetOrderShowList(OrderShowCond cond)
        {
            var dic = new Dictionary<string, List<OrderShowCaseSummary>>();

           
            var merchId = CurrentUser.MerchantId;
            var isMerch = CurrentUser.LoginType == LoginType.Merchant ? true : false;

            var baseQuery = (from o in baseRepository.GetList<Order>()
                             join ot in baseRepository.GetList<OrderDetail>()
                             on o.Id equals ot.OrderId
                             where 1 == 1
                             &&  o.IsActive && !o.IsDeleted
                             &&  o.IsActive && !o.IsDeleted
                             && (!isMerch || (isMerch && ot.MerchantId == merchId))
                             select new OrderShowCaseSummary
                             {
                                 Id = o.Id,
                                 OrderNO = o.OrderNO,
                                 CreateDate = o.CreateDate,
                                 UpdateDate = o.UpdateDate,
                                 OrderStatus = o.Status
                             }).Distinct();

            foreach (var item in cond.OrderCondList)
            {
                var list = GetLastedOrderList(baseQuery, item.TopQty, item.OrderStatus);
                dic.Add(item.TableType, list);
            }

            return dic;
        }

        /// <summary>
        /// 獲取待審批的產品列表
        /// </summary>
        /// <param name="getQty">需要獲取的數量</param>
        public List<ProductSummary> GetWaitingApproveProdLst(int getQty)
        {
            var prodList = new List<ProductSummary>();
            if (getQty > 0)
            {
                prodList = baseRepository.GetList<Product>(x => x.Status == ProductStatus.WaitingApprove && !x.IsDeleted && !x.IsApprove
                                       && x.Status == ProductStatus.WaitingApprove)
                                       .OrderByDescending(x => x.UpdateDate).Take(getQty)
                                       .Select(x => new ProductSummary
                                       {
                                           NameTransId = x.NameTransId,
                                           UpdateDate = x.UpdateDate,
                                           Code = x.Code,
                                           ProductId = x.Id,
                                           
                                       }).ToList();

                foreach (var item in prodList)
                {
                    //item.UpdateDateString = item.UpdateDate.Value.ToString(Runtime.Setting.DefaultDateTimeFormat);
                    item.Name = baseRepository.GetModel<Translation>(x => x.TransId == item.NameTransId && x.Lang == CurrentUser.Lang)?.Value ?? "";
                }
            }
            return prodList;
        }

        HotSalesSummaryView GenHotSaleView(List<HotSalesSummary> baseQuery, int topQty, TimePrecisionType timePrecision, SortType sortType)
        {
            var queryLst = new List<HotSalesSummary>();
            var hotView = new HotSalesSummaryView();

            string titleName = DateTime.Now.Date.ToString("MMMM", CultureInfo.GetCultureInfo("en-US"));

            DateTime today = DateTime.Now.Date;
            int thisDay = today.Day;//當前日
            int weekNum = Convert.ToInt32(today.DayOfWeek);//星期數
            int firstDay = today.AddDays(-weekNum + 1).Day;//周開始日
            int lastDay = today.AddDays(7 - weekNum).Day;//周結束日
            int thisWeek = GetWeekNumInMonth(today);//當前周

            if (topQty > 0)
            {
                switch (timePrecision)
                {
                    case TimePrecisionType.Week:
                        {
                            queryLst = baseQuery.Where(x => x.Day >= firstDay && x.Day <= lastDay).GroupBy(x => new { x.Year, x.Month, x.Sku })
                                                            .Select(s => new HotSalesSummary
                                                            {
                                                                Sku = s.Key.Sku,
                                                                Year = s.Key.Year,
                                                                Month = s.Key.Month,
                                                                Week = thisWeek,
                                                                TotalSalesQty = s.Sum(w => w.Qty)
                                                            }).ToList();
                            titleName = $"Week {queryLst?.FirstOrDefault()?.Week?.ToString() ?? "" }";
                        }
                        break;
                    case TimePrecisionType.Day:
                        {
                            queryLst = baseQuery.Where(x => x.Day == thisDay).GroupBy(x => new { x.Year, x.Month, x.Day, x.Sku })
                                                        .Select(s => new HotSalesSummary
                                                        {
                                                            Sku = s.Key.Sku,
                                                            Year = s.Key.Year,
                                                            Month = s.Key.Month,
                                                            Day = s.Key.Day,
                                                            TotalSalesQty = s.Sum(w => w.Qty)
                                                        }).ToList();
                            titleName = DateTime.Now.Date.ToString("MMMM", CultureInfo.GetCultureInfo("en-US")) + "-" + DateTime.Now.Date.Day.ToString().PadLeft(2, '0');
                        }
                        break;
                    default:
                        {
                            queryLst = baseQuery.GroupBy(x => new { x.Year, x.Month, x.Sku })
                                                         .Select(s => new HotSalesSummary
                                                         {
                                                             Sku = s.Key.Sku,
                                                             Year = s.Key.Year,
                                                             Month = s.Key.Month,
                                                             TotalSalesQty = s.Sum(w => w.Qty)
                                                         }).ToList();
                        }
                        break;
                }

                if (queryLst != null && queryLst.Any())
                {
                    //if (sortType == Model.Enums.SortType.DESC)
                    //{
                    //    queryLst = queryLst.OrderByDescending(x => x.TotalSalesQty).Take(topQty).ToList();
                    //}
                    //else
                    //{
                    //    queryLst = queryLst.OrderBy(x => x.TotalSalesQty).Take(topQty).ToList();
                    //}

                    queryLst = queryLst.AsQueryable().SortBy("TotalSalesQty", sortType).Take(topQty).ToList();

                    hotView.HotSalesDetailList = queryLst.Select(s => new HotSalesDetailView
                    {
                        ProductName = GetProductName(s.Sku),
                        Qty = s.Qty,
                        Week = s.Week
                    }).ToList();

                    hotView.TitleList.Add(titleName);
                }
            }

            return hotView;
        }

        List<OrderShowCaseSummary> GetLastedOrderList(IQueryable<OrderShowCaseSummary> baseQuery, int TopQty, OrderStatus orderStatus)
        {
            var query = new List<OrderShowCaseSummary>();

            if (TopQty > 0)
            {
                baseQuery = baseQuery.Where(x => x.OrderStatus == orderStatus);

                if (orderStatus == OrderStatus.ReceivedOrder)
                    baseQuery = baseQuery.OrderByDescending(x => x.CreateDate).Take(TopQty);
                else
                    baseQuery = baseQuery.OrderByDescending(x => x.UpdateDate).Take(TopQty);

                query = baseQuery.ToList();
            }
            return query;
        }

        private string GetProductName(Guid sku)
        {           
            string name = string.Empty;
            var product = (from s in baseRepository.GetList< ProductSku >()                       
                           join p in baseRepository.GetList<Product>()
                           on s.ProductCode equals p.Code
                           where s.IsActive && !s.IsDeleted
                           && p.Status == ProductStatus.OnSale && !p.IsDeleted  && s.Id == sku
                           select p).FirstOrDefault();
            if (product != null)
            {                
                name = baseRepository.GetModel<Translation>(x => x.TransId == product.NameTransId && x.Lang == CurrentUser.Lang)?.Value ?? "";
            }
            return name;
        }

        private int GetWeekNumInMonth(DateTime nowDate)
        {
            int dayInMonth = nowDate.Day;
            //本月第一日
            DateTime firstDay = nowDate.AddDays(1 - nowDate.Day);
            //本月第一日是星期幾
            int weekday = (int)firstDay.DayOfWeek == 0 ? 7 : (int)firstDay.DayOfWeek;
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
