using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;
using Intimex.Common;
using BDMall.Utility;

namespace BDMall.BLL
{
    public class ProductBLL : BaseBLL, IProductBLL
    {
        public IProductRepository productRepository;

        public ICurrencyBLL currencyBLL;

        public IAttributeBLL attributeBLL;

        public IProductCatalogBLL productCatalogBLL;

        public ITranslationRepository translationRepository;

        public IProductAttrBLL productAttrBLL;

        public ProductBLL(IServiceProvider services) : base(services)
        {
            productRepository = Services.Resolve<IProductRepository>();
            currencyBLL = Services.Resolve<ICurrencyBLL>();
            attributeBLL = Services.Resolve<IAttributeBLL>();
            translationRepository = Services.Resolve<ITranslationRepository>();
            productCatalogBLL = Services.Resolve<IProductCatalogBLL>();
            productAttrBLL = Services.Resolve<IProductAttrBLL>();
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

            result = productRepository.Search(cond);
            //var currency = CurrencyBLL.GetDefaultCurrency();
            //foreach (var item in result.Data)
            //{
            //    item.Imgs = GetProductImages(item.ProductId);
            //    item.IconRUrl = PathUtil.GetProductIconUrl(item.IconRType, CurrentUser.ComeFrom, CurrentUser.Language);
            //    //item.Currency = currency;
            //}
            return result;
        }

        public ProductEditModel GetProductInfo(Guid id)
        {
            var product = baseRepository.GetModelById<Product>(id);
            var result = GenProductEditModel(product);
            return result;
        }

        public List<string> GetProductImages(Guid prodID)
        {
            var productImages = new List<string>();

            var product = baseRepository.GetModelById<Product>(prodID);
            if (product != null)
            {
                var dbproductImage = baseRepository.GetModelById<ProductImage>(product.DefaultImage);
                if (dbproductImage != null)
                {
                    if (dbproductImage.ImageItems != null)
                    {
                        var imageItems = dbproductImage.ImageItems.OrderBy(o => o.Type).ToList();
                        if (imageItems != null)
                        {
                            var fileServer = string.Empty;
                            var activeImages = imageItems.Where(p => p.Path != null && p.Path != "").OrderBy(o => o.Type).ToList();
                            foreach (var item in imageItems)
                            {
                                if (!string.IsNullOrEmpty(item.Path))
                                {
                                    productImages.Add(fileServer + item.Path);
                                }
                                else
                                {
                                    productImages.Add(fileServer + imageItems[activeImages.Count - 1].Path);
                                }

                            }
                            if (productImages.Count < 8)//如果圖片不夠8個尺寸，最最大的尺寸補齊8張
                            {
                                int startI = productImages.Count;
                                for (int i = startI; i < 8; i++)
                                {
                                    productImages.Add(fileServer + imageItems[startI - 1].Path);
                                }
                            }
                        }
                    }

                }
            }
            return productImages;

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

        private ProductEditModel GenProductEditModel(Product product)
        {
            ProductEditModel productEditModel = new ProductEditModel();
           
            var merchantSupplierId = "";
            if (CurrentUser.IsMerchant)
            {
                var merchant = baseRepository.GetModel<Merchant>(x=>x.Id == CurrentUser.MerchantId);
                merchantSupplierId = merchant?.MerchNo ?? "";
            }

            if (product == null)
            {
                var supportLang = GetSupportLanguage();
                productEditModel.OriginalId = Guid.Empty;
                productEditModel.Id = Guid.NewGuid();
                productEditModel.MerchantSupplierId = merchantSupplierId;
                productEditModel.CurrencyCode = currencyBLL.GetDefaultCurrencyCode();               
                productEditModel.Name = "";              
                productEditModel.PageTitles = LangUtil.GetMutiLangFromTranslation(null, supportLang);
                productEditModel.ProductBriefs = LangUtil.GetMutiLangFromTranslation(null, supportLang);
                productEditModel.ProductDetail = LangUtil.GetMutiLangFromTranslation(null, supportLang);
                productEditModel.ProductNames = LangUtil.GetMutiLangFromTranslation(null, supportLang);
                productEditModel.SeoDescs = LangUtil.GetMutiLangFromTranslation(null, supportLang);
                productEditModel.SeoKeywords = LangUtil.GetMutiLangFromTranslation(null, supportLang);                                          
                productEditModel.InveAttrList = attributeBLL.GetInvAttributeByCatId(Guid.Empty);
                productEditModel.NonInveAttrList = attributeBLL.GetNonInvAttributeByCatId(Guid.Empty);             
            }
            else
            {
                var productStatistic = baseRepository.GetModel<ProductStatistics>(x => x.Code == product.Code);
                //var supportLang = GetSupportLanguage();
                productEditModel.OriginalId = product.FromProductId;
                productEditModel.Id = product.Id;
                productEditModel.MerchantId = product.MerchantId;
                productEditModel.MerchantSupplierId = merchantSupplierId;
                productEditModel.Code = product.Code;
                productEditModel.Category = product.CatalogId;
                productEditModel.CategoryPath = productCatalogBLL.GetCatalogPath(product.CatalogId);
                //productEditModel.CatTreeNodes = new List<ProdCatatogInfo>();
                productEditModel.Currency = currencyBLL.GetSimpleCurrency(product.CurrencyCode);
                productEditModel.CurrencyCode = product.CurrencyCode;
                productEditModel.Images = GetProductImages(product.Id);
                productEditModel.Name = translationRepository.GetDescForLang(product.NameTransId, CurrentUser.Lang);
                productEditModel.OriginalPrice = product.OriginalPrice;
                productEditModel.SalePrice = product.SalePrice;
                productEditModel.TimePrice = product.TimePrice;
                productEditModel.MarkupPrice = product.MarkUpPrice;
                productEditModel.TitleTransId = product.TitleTransId;
                productEditModel.PageTitles = translationRepository.GetMutiLanguage(product.TitleTransId);
                productEditModel.IntroductionTransId = product.IntroductionTransId;
                productEditModel.ProductBriefs = translationRepository.GetMutiLanguage(product.IntroductionTransId);
                productEditModel.DetailTransId = product.DetailTransId;
                productEditModel.ProductDetail = translationRepository.GetMutiLanguage(product.DetailTransId); 
                productEditModel.NameTransId = product.NameTransId;
                productEditModel.ProductNames = translationRepository.GetMutiLanguage(product.NameTransId);
                productEditModel.SeoDescTransId = product.SeoDescTransId;
                productEditModel.SeoDescs = translationRepository.GetMutiLanguage(product.SeoDescTransId);
                productEditModel.KeyWordTransId = product.KeyWordTransId;
                productEditModel.SeoKeywords = translationRepository.GetMutiLanguage(product.KeyWordTransId);
                productEditModel.SpecifExtension = GetProductExtension(product.Id);
                productEditModel.CommissionConfig = GetProductCommissionByProdId(product.Id);
                productEditModel.Specification = GetProductSpecification(product.Id);
                productEditModel.DefaultImage = product.DefaultImage;
                productEditModel.VisitCounter = productStatistic == null ? 0 : productStatistic.VisitCounter;
                productEditModel.PurchaseCounter = productStatistic == null ? 0 : productStatistic.PurchaseCounter;
                productEditModel.Remark = product.Remark;
                productEditModel.InveAttrList = productAttrBLL.GetInvAttributeByProductMap(product.Id);
                productEditModel.NonInveAttrList = productAttrBLL.GetNonInvAttributeByProductMap(product.Id);
                productEditModel.IsApprove = product.IsApprove;
                productEditModel.IsExistInvRec = CheckProductHasInventory(product.Id);
                productEditModel.IsActive = product.IsActive;
                productEditModel.ActiveTimeFrom = product.ActiveTimeFrom;
                productEditModel.ActiveTimeTo = product.ActiveTimeTo;
                productEditModel.CountryIds = baseRepository.GetList<ProductRefuseDelivery>(x=>x.ProductId == product.Id).Select(c=>c.CountryId).ToList();
                productEditModel.GS1Status = productEditModel.SpecifExtension.GS1Status;
                productEditModel.Status = product.Status;
               
                if (!string.IsNullOrEmpty(product.Remark))
                {
                    var saleTimes = product.Remark.Trim().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (saleTimes.Length >= 2)
                    {
                        var time = DateUtil.ConvertoDateTime(saleTimes[1], "yyyy-MM-dd HH:mm:00");
                        productEditModel.SaleTime = time;
                    }
                    else
                    {
                        productEditModel.SaleTime = null;
                    }
                }
            }

            return productEditModel;
        }

        bool CheckProductHasInventory(Guid prodID)
        {
            var invCount = (from a in baseRepository.GetList<Product>()
                            join s in baseRepository.GetList<ProductSku>() on new { a1 = a.Code, a2 = true, a3 = false } equals new { a1 = s.ProductCode, a2 = s.IsActive, a3 = s.IsDeleted }
                            join i in baseRepository.GetList<Inventory>() on s.Id equals i.Sku
                            where a.Id == prodID && i.Quantity > 0
                            select 1).Any();

            return invCount;

        }

        private ProductSpecificationView GetProductSpecification(Guid id)
        {
            ProductSpecificationView view = new ProductSpecificationView();
            var specification = baseRepository.GetModelById<ProductSpecification>(id);
            if (specification != null)
            {
                view.Heigth = specification.ProductDimension.Heigth;
                view.Width = specification.ProductDimension.Width;
                view.Length = specification.ProductDimension.Length;
                view.Unit = specification.ProductDimension.Unit;
                view.PackageDescription = specification.PackageDescription;
                view.PackageHeight = specification.ProductPackage.Heigth;
                view.PackageWidth = specification.ProductPackage.Width;
                view.PackageLength = specification.ProductPackage.Length;
                view.PackageUnit = specification.ProductPackage.Unit;
                view.GrossWeight = specification.GrossWeight;
                view.NetWeight = specification.NetWeight;
                view.WeightUnit = specification.WeightUnit;
                view.ProdID = specification.Id;
            }
            return view;
        }

        private ProductCommissionView GetProductCommissionByProdId(Guid prodId)
        {
            ProductCommissionView view = new ProductCommissionView()
            {
                ProductId = prodId,
                CMCalType = ProdCommissionType.MerchInheriting
            };

            var entity = baseRepository.GetList<ProductCommission>(x => x.ProductId == prodId && x.IsActive && !x.IsDeleted).OrderByDescending(x=>x.CreateDate).FirstOrDefault();
            if (entity != null)
            {
                view.Id = entity.Id;
                view.CMVal = entity.CMVal;
                view.CMRate = entity.CMRate;
                view.CMCalType = entity.CMCalType;
            }

            return view;
        }

        private ProductExtensionView GetProductExtension(Guid id)
        {
            ProductExtensionView view = new ProductExtensionView();
            var extension = baseRepository.GetModelById<ProductExtension>(id);
            if (extension != null)
            {
                view.IsOnSale = extension.IsOnSale;
                view.IsSaleOff = extension.IsSaleOff;
                view.MaxPurQty = extension.MaxPurQty;
                view.MinPurQty = extension.MinPurQty;
                view.PermissionLevel = extension.PermissionLevel;
                view.ProdID = extension.Id;
                view.YoutubeLink = extension.YoutubeLink;
                view.YoukuLink = extension.YoukuLink;
                view.SafetyStock = extension.SafetyStock;
                view.ProductType = extension.ProductType;
                view.IsLimit = extension.IsLimit;
                view.NoRefund = extension.IsSalesReturn;
                view.HSCode = extension.HSCode;
                view.GS1Status = extension.GS1Status;
            }

            return view;
        }
    }
}
