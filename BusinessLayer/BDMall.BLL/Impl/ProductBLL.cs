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
using System.IO;

namespace BDMall.BLL
{
    public class ProductBLL : BaseBLL, IProductBLL
    {
        public IProductRepository productRepository;
        public ICurrencyBLL currencyBLL;
        public IAttributeBLL attributeBLL;
        public IProductCatalogBLL productCatalogBLL;
        public ITranslationRepository translationRepository;
        public IProductImageRepository productImageRepository;
        public IProductAttrBLL productAttrBLL;
        public IMerchantShipMethodMappingRepository merchantShipMethodMappingRepository;
        public IProductImageBLL productImageBLL;
        public ISettingBLL settingBLL;
        public IProductSkuRepository productSkuRepository;
        public IProductDetailRepository productDetailRepository;
        
        public PreHeatProductService productService;
        public PreHeatProductImageService productImageService;
        public PreHeatProductStaticsService productStatisticsService;
        public PreHeatProductSkuService  productSkuService; 

        public ProductBLL(IServiceProvider services) : base(services)
        {
            productRepository = Services.Resolve<IProductRepository>();
            currencyBLL = Services.Resolve<ICurrencyBLL>();
            attributeBLL = Services.Resolve<IAttributeBLL>();
            translationRepository = Services.Resolve<ITranslationRepository>();
            productCatalogBLL = Services.Resolve<IProductCatalogBLL>();
            productAttrBLL = Services.Resolve<IProductAttrBLL>();
            productImageRepository = Services.Resolve<IProductImageRepository>();
            merchantShipMethodMappingRepository = Services.Resolve<IMerchantShipMethodMappingRepository>();
            productSkuRepository = Services.Resolve<IProductSkuRepository>();
            productImageBLL = Services.Resolve<IProductImageBLL>();
            settingBLL = Services.Resolve<ISettingBLL>();
            productDetailRepository = Services.Resolve<IProductDetailRepository>();

            productService = (PreHeatProductService)Services.GetService(typeof(PreHeatProductService));
            productImageService = (PreHeatProductImageService)Services.GetService(typeof(PreHeatProductImageService));
            productStatisticsService = (PreHeatProductStaticsService)Services.GetService(typeof(PreHeatProductStaticsService));
            productSkuService = (PreHeatProductSkuService)Services.GetService(typeof(PreHeatProductSkuService));
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
                             where ps.IsActive && ps.IsDeleted == false && (!isMerch || (isMerch && ps.MerchantId == merchId))
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
            foreach (var item in result.Data)
            {
                item.Imgs = GetProductImages(item.ProductId);
                item.ImgPath = item.Imgs.FirstOrDefault() ?? "";  
                //item.IconRUrl = PathUtil.GetProductIconUrl(item.IconRType, CurrentUser.ComeFrom, CurrentUser.Language);
                //item.Currency = currency;
            }
            return result;
        }

        public PageData<ProductSummary> SearchProductList(ProdSearchCond cond)
        { 
            var result  = SearchBackEndProductSummary(cond);
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
                    var imageItems = baseRepository.GetList<ProductImageList>(x => x.ImageID == dbproductImage.Id).OrderBy(o => o.Type).ToList();
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
            return productImages;
        }

        public SystemResult CheckTimePriceByCode(string code, Guid MerchantId)
        {
            SystemResult result = new SystemResult();
            var productpricehourList = baseRepository.GetList<ProductPriceHour>(d => !d.IsDeleted && d.ProductCode == code
                                                        && d.MerchantId == MerchantId && d.IsActive && !d.IsTimeStatus)?.OrderByDescending(d => d.BeginTime).FirstOrDefault();
            if (productpricehourList == null) return result;

            result.ReturnValue = productpricehourList;
            result.Succeeded = true;

            return result;
        }

        public ProductDto SaveProduct(ProductEditModel product)
        {
            if (!CurrentUser.IsMerchant) throw new BLException(BDMall.Resources.Message.NotMerchantToOperate);
            var wordQtyCheck = ContentWordQtyCheck(product);
            if (!wordQtyCheck.Succeeded) throw new BLException(wordQtyCheck.Message);

            if (product.Action == ActionTypeEnum.Add.ToString() || product.Action == ActionTypeEnum.Copy.ToString())
            {
                if (CheckProductIsExists(product.MerchantSupplierId.Trim() + product.Code.Trim()))
                {
                    throw new BLException(BDMall.Resources.Message.ProductIsExists);
                }
            }

            Product dbProduct = new Product();
            if (product.Action == ActionTypeEnum.Add.ToString() || product.Action == ActionTypeEnum.Copy.ToString() || product.Action == ActionTypeEnum.NewVer.ToString())
            {
                dbProduct = GenProduct(null, product);
            }
            else
            {
                var aProduct = baseRepository.GetModelById<Product>(product.Id);
                dbProduct = GenProduct(aProduct, product);
            }

            if (product.ActiveTimeTo < DateTime.Now)
            {
                throw new BLException(BDMall.Resources.Message.DateCompareToday);
            }

            UnitOfWork.IsUnitSubmit = true;
            if (product.Action == ActionTypeEnum.Add.ToString() || product.Action == ActionTypeEnum.Copy.ToString() || product.Action == ActionTypeEnum.NewVer.ToString())
            {
                if (product.Action == ActionTypeEnum.Copy.ToString() || product.Action == ActionTypeEnum.NewVer.ToString())//創建新版本產品時判斷
                {
                    product.OriginalId = product.Id;
                    product.Id = dbProduct.Id;// dbProduct.Id;
                    CopyProductImageFromDestProduct(product);//复制原产品的产品图片
                    dbProduct.DefaultImage = product.DefaultImage;
                    dbProduct.FromProductId = product.OriginalId;
                    CopyProductRelationProduct(product);//复制原产品的关联产品
                }
                else if (product.Action == ActionTypeEnum.Add.ToString())
                {
                    product.Id = dbProduct.Id;
                }

                product.DetailTransId = Guid.NewGuid();
                dbProduct.DetailTransId = product.DetailTransId;

                baseRepository.Insert(dbProduct);

                InsertOrUpdateProductTranslation(dbProduct, product, ActionTypeEnum.Add);
                InsertOrUpdateProductDetail(product, ActionTypeEnum.Add);               
                InsertOrUpdateInvProductAttribute(dbProduct, product, ActionTypeEnum.Add);//插入库存属性并新增SKU
                InsertOrUpdateNonInvProductAttribute(dbProduct, product, ActionTypeEnum.Add);
                InsertOrUpdateProductExtension(product, ActionTypeEnum.Add);
                InsertOrUpdateProductCommission(product, ActionTypeEnum.Add);
                InsertOrUpdateProductSpecification(product, ActionTypeEnum.Add);
                InsertOrUpdateProductRefuseDelivery(product);

                if (product.Action == ActionTypeEnum.Add.ToString() || product.Action == ActionTypeEnum.Copy.ToString())
                    InsertProductStatistics(dbProduct);

                if (product.Action == ActionTypeEnum.NewVer.ToString())//新版只留最新的产品记录，其它记录失效
                {
                    var oldProducts = baseRepository.GetList<Product>(p => p.Code == dbProduct.Code && p.Status != ProductStatus.OnSale).ToList();
                    foreach (var item in oldProducts)
                    {
                        item.IsActive = false;
                    }
                    baseRepository.Update(oldProducts);    

                    var oldStatics = baseRepository.GetModel<ProductStatistics>(p => p.Code == dbProduct.Code);
                    if (oldStatics != null) oldStatics.InternalNameTransId = dbProduct.NameTransId;
                    baseRepository.Update(oldStatics);
                }
            }
            else  //编辑
            {
                InsertOrUpdateProductTranslation(dbProduct, product, ActionTypeEnum.Modify);                
                InsertOrUpdateInvProductAttribute(dbProduct, product, ActionTypeEnum.Modify);
                InsertOrUpdateNonInvProductAttribute(dbProduct, product, ActionTypeEnum.Modify);
                InsertOrUpdateProductDetail(product, ActionTypeEnum.Modify);
                InsertOrUpdateProductExtension(product, ActionTypeEnum.Modify);
                InsertOrUpdateProductCommission(product, ActionTypeEnum.Modify);
                InsertOrUpdateProductSpecification(product, ActionTypeEnum.Modify);
                InsertOrUpdateProductRefuseDelivery(product);

                var defaultImage = baseRepository.GetModel<ProductImage>(x => x.Id == dbProduct.DefaultImage);
                if (defaultImage == null || defaultImage.IsDeleted)
                {
                    var earliestImage = productImageRepository.GetImageByType(dbProduct.Id, ImageType.SkuImage).OrderBy(o => o.CreateDate).FirstOrDefault();
                    if (earliestImage != null)
                    {
                        dbProduct.DefaultImage = earliestImage.Id;
                    }
                }

                baseRepository.Update(dbProduct);
                UpdateProductHourPriceRemak(product);
            }
            product.Code = dbProduct.Code;
            product.MerchantId = dbProduct.MerchantId;
            InsertOrUpdateFreeChargeProduct(product);

            UnitOfWork.Submit();

            var dto = AutoMapperExt.MapTo<ProductDto>(dbProduct);
            return dto;
        }

        public async Task UpdateCache(string Code, ProdAction action)
        {
            string key = $"{PreHotType.Hot_PromotionProduct}";

            if (action >= ProdAction.Delete)
            {
                ////根据Code，删除BD_Hot_PromotionProduct缓存                
                //await promotionProductService.DeletePromotionProduct(key, Code);

                //更新Product商品缓存（不能删除,为保证数据一致性），如：状态,ProductId等字段
                key = $"{PreHotType.Hot_Products}";
                await productService.UpdateProductWhenOffSale(key, Code);
            }
            else
            {
                var product = await baseRepository.GetModelAsync<Product>(x => x.Code == Code && x.IsActive && !x.IsDeleted && x.Status == ProductStatus.OnSale);
                if (product != null)
                {
                    //更新Product商品缓存
                    await productService.CreatePreHeat(product.Id);

                    ////更新PromotionProduct缓存
                    //await productService.UpdatePromotionProduct(key, Code);

                    //更新图片缓存
                    await productImageService.CreatePreHeat(product.Id);

                    //传入ProductId,在里面转换
                    await productStatisticsService.CreatePreHeat(product.Id);

                    //更新ProductSku缓存
                    await productSkuService.CreatePreHeat(product.Id);

                    #region 更新商品的可配送区域

                    key = CacheKey.RefuseCountries.ToString();
                    await RedisHelper.HDelAsync(key, product.Code);

                    key = CacheKey.SupportCountries.ToString();
                    await RedisHelper.HDelAsync(key, product.Code);

                    await GenProductDeliveryArea(product.Id, product.Code);

                    #endregion
                }
            }
        }

        public async Task<List<string>> CopyProductImageToPath(ProductEditModel product)
        {
            var insertProductImages = new List<string>();

            var destProductImages = productImageBLL.GetImageByProductId(product.OriginalId).ToList();
            var targetProductImages = productImageBLL.GetImageByProductId(product.Id).ToList();

            string destPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], product.MerchantId.ToString(), FileFolderEnum.Product) + "\\" + product.OriginalId;
            string defaultImgPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], product.MerchantId.ToString(), FileFolderEnum.DefaultImage);
            string targetPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], product.MerchantId.ToString(), FileFolderEnum.Product) + "\\" + product.Id;

            if (destProductImages != null)
            {
                var skuImages = destProductImages.Where(p => p.Type == ImageType.SkuImage).ToList();
                var additionalImages = destProductImages.Where(p => p.Type == ImageType.AdditionImage).ToList();

                foreach (var item in skuImages)//複製SKU圖片
                {
                    if (item.Items != null)
                    {
                        foreach (var destImage in item.Items)
                        {
                            var targetImage = targetProductImages.FirstOrDefault(p => p.AttrValue1 == item.AttrValue1 && p.AttrValue2 == item.AttrValue2 && p.AttrValue3 == item.AttrValue3 && p.Type == ImageType.SkuImage);
                            if (targetImage != null)
                            {
                                if (targetImage.Items != null)
                                {
                                    var image = targetImage.Items.FirstOrDefault(p => p.Type == destImage.Type);
                                    if (image != null)
                                    {
                                        var sourceImg = "";
                                        if (destImage.Path.Contains("Default"))
                                        {
                                            sourceImg = defaultImgPath;
                                        }
                                        else
                                        {
                                            sourceImg = destPath;
                                        }
                                        FileUtil.CopyFile(Path.Combine(sourceImg, Path.GetFileName(destImage.Path)), targetPath, Path.GetFileName(image.Path));

                                        insertProductImages.Add(image.Path);
                                    }
                                }

                            }
                        }
                    }
                }

                //复制附加图片
                var additionalImageSkus = additionalImages.Select(d => new
                {
                    attrValue1 = d.AttrValue1,
                    attrValue2 = d.AttrValue2,
                    attrValue3 = d.AttrValue3
                }).Distinct().ToList();

                foreach (var item in additionalImageSkus)
                {
                    //獲取sku對應的附加圖片
                    var destAdditionalImages = additionalImages.Where(p => p.AttrValue1 == item.attrValue1 && p.AttrValue2 == item.attrValue2 && p.AttrValue3 == item.attrValue3).ToList();

                    for (int i = 0; i < destAdditionalImages.Count; i++)
                    {
                        var addImage = destAdditionalImages[i];
                        if (addImage.Items != null)
                        {
                            var targetImages = targetProductImages.Where(p => p.AttrValue1 == item.attrValue1 && p.AttrValue2 == item.attrValue2 && p.AttrValue3 == item.attrValue3 && p.Type == ImageType.AdditionImage).ToList();

                            if (targetImages.Count > 0)
                            {
                                var targetImage = targetImages[i];

                                foreach (var destImage in addImage.Items)
                                {
                                    if (targetImage != null)
                                    {
                                        if (targetImage.Items != null)
                                        {
                                            var image = targetImage.Items.FirstOrDefault(p => p.Type == destImage.Type);
                                            if (image != null)
                                            {
                                                FileUtil.CopyFile(Path.Combine(destPath, Path.GetFileName(destImage.Path)), targetPath, Path.GetFileName(image.Path));
                                                insertProductImages.Add(image.Path);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }

            return insertProductImages;
        }

        public async Task CreateDefaultImage(ProductEditModel product)
        {
            List<KeyValue > dbImagePaths = new List<KeyValue>();

            var imageSizes = settingBLL.GetProductImageSize();

            var sourcePath = PathUtil.GetDefaultImagePath();

            GenSkuImagePath(product, dbImagePaths, imageSizes);

            ProductImageCondition cond = new ProductImageCondition();
            cond.AttrValue1 = Guid.Empty;
            cond.AttrValue2 = Guid.Empty;
            cond.AttrValue3 = Guid.Empty;
            cond.ImageType = ImageType.SkuImage;
            cond.ProdId = product.Id;

            cond.ImagePaths = dbImagePaths;
            
            string tempPath =PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], product.MerchantId.ToString(), FileFolderEnum.TempPath);
            string targetPath =PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], product.MerchantId.ToString(), FileFolderEnum.Product) + "\\" + product.Id;
            List<string> insertImgs = new List<string>();

            if (dbImagePaths != null && dbImagePaths.Any())
            {
                for (int i = 0; i < imageSizes.Count; i++)
                {
                    var img = dbImagePaths[i];
                    var imgSize = imageSizes[i];
                    insertImgs.Add(img.Text);
                    ImageUtil.CreateImg(sourcePath, targetPath, Path.GetFileName(img.Text), imgSize.Width, imgSize.Length);
                }
            }

            productImageBLL.SaveProductImage(cond);
           
        }

        public PageData<ProductSummary> SearchRelatedProduct(RelatedProductCond cond)
        {
            PageData<ProductSummary> result = new PageData<ProductSummary>();
            var products = productRepository.SearchRelatedProduct(cond);

            result.TotalRecord = products.TotalRecord;
            result.Data = products.Data.Select(d => GenProductSummary(d)).ToList();

            return result;
        }

        public List<ProductSummary> GetRelatedProduct(Guid id)
        {

            List<ProductSummary> result = new List<ProductSummary>();
            var products = productRepository.GetRelatedProduct(id);

            result = products.Select(d => GenProductSummary(d)).ToList();
            return result;
        }

        public void AddRelatedProduct(List<string> prodCodes, Guid originalId)
        {
            if (prodCodes != null && prodCodes.Any())
            {
                var product = baseRepository.GetModelById<Product>(originalId);
                if (product != null)
                {                   
                    var list = prodCodes.Select(item => new ProductRelatedItem
                    {
                        Id = Guid.NewGuid(),
                        ItemId = baseRepository.GetModel<Product>(x => x.Code == item && x.Status == ProductStatus.OnSale && !x.IsDeleted)?.Id ?? Guid.Empty,
                        ItemCode = item,
                        ProductId = originalId,
                        Seq = 0,
                    }).ToList();

                    baseRepository.Insert(list);
                }               
            }
        }

        public void DeleteRelatedProduct(Guid prodId, List<string> prodCodes)
        {
            if (prodCodes != null)
            {
                var product = baseRepository.GetModel<Product>(x => x.Id == prodId);
                if (product != null)
                {
                    var list = baseRepository.GetList<ProductRelatedItem>(p => p.IsActive && !p.IsDeleted && p.ProductId == prodId && prodCodes.Contains(p.ItemCode));
                    foreach (var item in list)
                    {
                        item.IsDeleted = true;
                    }
                    baseRepository.Update(list);
                }
            }
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="prodIDs"></param>
        /// <returns></returns>
        public async Task ProductLogicalDelete(List<string> prodIDs)
        {
            if (prodIDs != null && prodIDs.Any())
            {
                foreach (var item in prodIDs)
                {
                    var product = baseRepository.GetModel<Product>(x=>x.Id == Guid.Parse(item));
                    if (product != null)
                    {
                        product.IsDeleted = true;
                        if (product.Status == ProductStatus.OnSale) product.Status = ProductStatus.ManualOffSale;
                    }
                    baseRepository.Update(product);
                    await UpdateCache(product.Code, ProdAction.Delete);
                }
            }
        }

        /// <summary>
        /// 设置为上架
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="BLException"></exception>
        public async Task ActiveProducts(List<string> ids)
        {
            var productList = ids.Select(s=>Guid.Parse(s)).ToList();
            var wrongStatus = baseRepository.GetList<Product>(p=>productList.Contains(p.Id) && (p.Status != ProductStatus.ManualOffSale 
                                && p.Status != ProductStatus.Editing && p.Status != ProductStatus.AutoOffSale)).Select(s=>s.Code).ToList();
            if (wrongStatus !=null && wrongStatus.Any())
                throw new BLException(string.Format(Resources.Message.PleaseSelectOffSaleProduct, string.Join(",", wrongStatus.ToArray())));

            var products = baseRepository.GetList<Product>(p => productList.Contains(p.Id)).ToList();
            foreach (var item in products)
            {
                item.UpdateDate = DateTime.Now;
                if (item.ActiveTimeFrom > DateTime.Now || item.ActiveTimeTo < DateTime.Now)                
                    item.Status = ProductStatus.WaitingOnSale;                
                else               
                    item.Status = ProductStatus.OnSale;              
            }
            baseRepository.Update(products);

            foreach (var item in products)
            {
                 if (item.Status == ProductStatus.OnSale) await UpdateCache(item.Code, ProdAction.OnSale);
            }
        }

        /// <summary>
        /// 设置为下架
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="BLException"></exception>
        public async Task DisActiveProducts(List<string> ids)
        {
            var productList = ids.Select(s => Guid.Parse(s)).ToList();
            var wrongStatus = baseRepository.GetList<Product>(p => productList.Contains(p.Id) && p.Status != ProductStatus.OnSale).Select(s => s.Code).ToList();
            if (wrongStatus != null && wrongStatus.Any())
                throw new BLException(string.Format(Resources.Message.PleaseSelectOnSaleProduct, string.Join(",", wrongStatus.ToArray())));

            var products = baseRepository.GetList<Product>(p => productList.Contains(p.Id)).ToList();

            foreach (var item in products)
            {
                item.Status = ProductStatus.ManualOffSale;
                item.UpdateDate = DateTime.Now;
            }
            baseRepository.Update(products);

            foreach (var item in products)
            {
                await UpdateCache(item.Code, ProdAction.OffSale);
            }
        }

        /// <summary>
        /// 将产品状品改为待审批
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SystemResult ApplyApprove(Guid id)
        {
            SystemResult result = new SystemResult();

            UnitOfWork.IsUnitSubmit = true;
            var product = baseRepository.GetModelById<Product>(id);
            if (product != null)
            {
                product.Status = ProductStatus.WaitingApprove;
                baseRepository.Update(product);
                InsertProductApproveHistory(id, ApproveResult.WaitingApprove, "");
            }
            UnitOfWork.Submit();

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 拒絕通過產品
        /// </summary>
        /// <param name="prodID"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task TurndownProduct(Guid prodID, string reason)
        {
            UnitOfWork.IsUnitSubmit = true;
            var product = baseRepository.GetModelById<Product>(prodID);
            if (product != null)
            {
                product.IsApprove = true;
                product.Status = ProductStatus.Reject;
            }

            baseRepository.Update(product);
            InsertProductApproveHistory(prodID, ApproveResult.Reject, reason);
            UnitOfWork.Submit();

            //发送邮件
        }

        public void InsertProductApproveHistory(Guid productId, ApproveResult type, string remark)
        {
            ApproveHistory obj = new ApproveHistory();

            obj.Id = Guid.NewGuid();
            obj.ApproveType = type;
            obj.MerchantId = CurrentUser.MerchantId;
            obj.OperateDate = DateTime.Now;
            obj.OperateId = Guid.Parse(CurrentUser.UserId);
            obj.ModuleId = productId;
            obj.ApproveModule = ApproveModule.Product;
            obj.Remark = remark;

            baseRepository.Insert(obj);
        }

        public ProductSkuDto GetProductSku(Guid skuId)
        {
            var dbsku = baseRepository.GetModelById<ProductSku>(skuId);
            if (dbsku == null) return null;

            var sku = AutoMapperExt.MapTo<ProductSkuDto>(dbsku);

            var attr1 = attributeBLL.GetAttribute(sku.Attr1);
            var attr2 = attributeBLL.GetAttribute(sku.Attr2);
            var attr3 = attributeBLL.GetAttribute(sku.Attr3);
            sku.Attr1Name = attr1?.Desc ?? "";
            sku.Attr2Name = attr2?.Desc ?? "";
            sku.Attr3Name = attr3?.Desc ?? "";
            sku.AttrValue1Name = attr1?.AttributeValues == null ? "" : attr1?.AttributeValues.FirstOrDefault(p => p.Id == sku.AttrValue1)?.Desc ?? "";
            sku.AttrValue2Name = attr2?.AttributeValues == null ? "" : attr2?.AttributeValues.FirstOrDefault(p => p.Id == sku.AttrValue2)?.Desc ?? "";
            sku.AttrValue3Name = attr3?.AttributeValues == null ? "" : attr3?.AttributeValues.FirstOrDefault(p => p.Id == sku.AttrValue3)?.Desc ?? "";

            return sku;

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
                productEditModel.ProductDetail = productDetailRepository.GetMutiLanguage(product.DetailTransId); 
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

        /// <summary>
        /// 檢查產品是否有庫存記錄
        /// </summary>
        /// <param name="prodID"></param>
        /// <returns></returns>
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
                view.Heigth = specification.Heigth;
                view.Width = specification.Width;
                view.Length = specification.Length;
                view.Unit = specification.Unit;
                view.PackageDescription = specification.PackageDescription;
                view.PackageHeight = specification.PackageHeigth;
                view.PackageWidth = specification.PackageWidth;
                view.PackageLength = specification.PackageLength;
                view.PackageUnit = specification.PackageUnit;
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

        private async Task<Tuple<List<ProductDeliveryArea>, List<ProductDeliveryArea>>> GenProductDeliveryArea(Guid ProductId, string Code)
        {
            string key = $"{CacheKey.RefuseCountries}";
            var refuseCountries = (await RedisHelper.HGetAsync<List<ProductDeliveryArea>>(key, Code))?.AsQueryable();
            if (refuseCountries == null || !refuseCountries.Any())
            {
                refuseCountries = from r in baseRepository.GetList<ProductRefuseDelivery>()
                                  join c in baseRepository.GetList<Country>() on r.CountryId equals c.Id
                                  where r.ProductId == ProductId && r.IsActive && !r.IsDeleted
                                  select new ProductDeliveryArea { ProductId = r.ProductId, Code = Code, Country = c };

                if (refuseCountries != null && refuseCountries.Any())
                    await RedisHelper.HSetAsync(key, Code, refuseCountries.ToList());       //最好能设置过期时间
            }

            key = $"{CacheKey.SupportCountries}";
            var supportCountries = (await RedisHelper.HGetAsync<List<ProductDeliveryArea>>(key, Code))?.AsQueryable();
            if (supportCountries == null || !supportCountries.Any())
            {
                supportCountries = baseRepository.GetList<Country>()
                        .Where(x => x.IsActive && !x.IsDeleted && !refuseCountries.Select(s => s.Country.Id).Any(c => c.Equals(x.Id)))
                        .OrderBy(o => o.Id).Select(s => new ProductDeliveryArea { ProductId = ProductId, Code = Code, Country = s });

                if (supportCountries != null && supportCountries.Any())
                    await RedisHelper.HSetAsync(key, Code, supportCountries.ToList());      //最好能设置过期时间                
            }

            var result = new Tuple<List<ProductDeliveryArea>, List<ProductDeliveryArea>>(refuseCountries.ToList(), supportCountries.ToList());
            return result;
        }

        /// <summary>
        /// 檢查產品詳細內容的數字是否有超過限制數值
        /// </summary>
        /// <param name="product">產品詳細內容</param>
        private SystemResult ContentWordQtyCheck(ProductEditModel product)
        {
            var sysRslt = new SystemResult();
            sysRslt.Succeeded = true;

            if (product != null)
            {
                string overFlowFmt = Resources.Message.DataLengthOverFlow;
                int wordQtyLimited = Runtime.Setting.UnlimitedContentWordQty;

                if (product.ProductDetail?.Count > 0)
                {
                    foreach (var item in product.ProductDetail)
                    {
                        if (item.Desc?.Length > wordQtyLimited)
                        {
                            sysRslt.Message = string.Format(overFlowFmt, BDMall.Resources.Label.ProductDetail, wordQtyLimited.ToString());
                            sysRslt.Succeeded = false;
                            return sysRslt;
                        }
                    }
                }
            }

            return sysRslt;
        }

        public bool CheckProductIsExists(string code)
        {
            var flag = baseRepository.Any<Product>(x => x.Code == code && x.IsActive && !x.IsDeleted);
            return flag;
        }

        public ProductSummary GetProductSummary(Guid id, Guid skuId)
        {        
            var product = baseRepository.GetModelById<Product>(id);
            if (product == null)    return null; 
            var result = GenProductSummary(product, skuId);
            return result;
        }

        /// <summary>
        /// 获取SaleQty<0的数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetSelloutSkus()
        {
            string SalesQtyKey = $"{CacheKey.SalesQty}";
            //优先读缓存
            var cacheData =  await RedisHelper.ZRangeByScoreAsync(SalesQtyKey, "-inf", "0");   //取小于等于0           
            if (cacheData?.Any() ?? false)
            {
                var query = from q in UnitOfWork.DataContext.ProductSkus
                            join s in UnitOfWork.DataContext.ProductQties on q.Id equals s.SkuId into qss
                            from qs in qss.DefaultIfEmpty()
                            where qs == null || (q.IsActive && q.IsDeleted == false && qs.SalesQty <= 0)
                            select q.Id;
                cacheData = query.Select(d => d.ToString()).ToArray();
            }
            return cacheData.ToList();
        }

        private Product GenProduct(Product dbProduct, ProductEditModel viewProduct)
        {
            Product product = new Product();

            if (dbProduct != null) product = dbProduct;

            bool isNew = false;
            if (viewProduct.Action == ActionTypeEnum.Add.ToString() || viewProduct.Action == ActionTypeEnum.Copy.ToString() || viewProduct.Action == ActionTypeEnum.NewVer.ToString())
            {
                isNew = true;
                product.MerchantId = CurrentUser.MerchantId;
                if (viewProduct.Action == ActionTypeEnum.Add.ToString() || viewProduct.Action == ActionTypeEnum.Copy.ToString())
                {
                    product.Code = viewProduct.MerchantSupplierId.Trim() + viewProduct.Code.Trim();

                }
                else
                {
                    product.Code = viewProduct.Code.Trim();
                }

                product.Id = Guid.NewGuid();
            }
            else
            {
                isNew = false;
                product.MerchantId = viewProduct.MerchantId;
                product.Id = viewProduct.Id;
            }

            product.ActiveTimeFrom = viewProduct.ActiveTimeFrom;
            product.ActiveTimeTo = viewProduct.ActiveTimeTo;
            product.CatalogId = viewProduct.Category;
            product.CurrencyCode = viewProduct.CurrencyCode ?? "";
            product.NameTransId = isNew ? Guid.NewGuid() : viewProduct.NameTransId;
            product.TitleTransId = isNew ? Guid.NewGuid() : viewProduct.TitleTransId;
            product.KeyWordTransId = isNew ? Guid.NewGuid() : viewProduct.KeyWordTransId;
            product.SeoDescTransId = isNew ? Guid.NewGuid() : viewProduct.SeoDescTransId;
            product.IntroductionTransId = isNew ? Guid.NewGuid() : viewProduct.IntroductionTransId;
            product.DetailTransId = isNew ? Guid.NewGuid() : viewProduct.DetailTransId;
            //product.IngredientTransId = isNew ? Guid.NewGuid() : viewProduct.IngredientTransId;
            //product.InstructionsTransId = isNew ? Guid.NewGuid() : viewProduct.InstructionsTransId;

            product.IsActive = true;
            product.DefaultImage = isNew ? Guid.Empty : viewProduct.DefaultImage;
            product.FromProductId = viewProduct.OriginalId;
            product.Remark = viewProduct.Remark;
            //保存时SalePrice修改以TimePrice为准
            product.SalePrice = viewProduct.TimePrice;
            product.OriginalPrice = viewProduct.OriginalPrice;
            product.MarkUpPrice = viewProduct.MarkupPrice;
            product.Status = ProductStatus.Editing;
            product.IsApprove = true;
            product.IsDeleted = false;
            product.CreateDate = DateTime.Now;
            product.UpdateDate = DateTime.Now;
            product.CreateBy = Guid.Parse(CurrentUser.UserId);
            product.UpdateBy = Guid.Parse(CurrentUser.UserId);
            product.Remark = "SaleTime|" + DateUtil.DateTimeToString(viewProduct.SaleTime, "yyyy-MM-dd HH:mm:00");

            product.TimePrice = viewProduct.TimePrice;
            return product;
        }

        /// <summary>
        /// 复制原产品的产品图片、附加图片
        /// </summary>
        private void CopyProductImageFromDestProduct(ProductEditModel product)
        {
            var insertProductImages = new List<ProductImage>();
            var invAttrs = product.InveAttrList;
            var oProductImages = productImageRepository.GetImageByProductId(product.OriginalId).OrderByDescending(o => o.Type).OrderBy(d => d.CreateDate).ToList();

            foreach (var item in oProductImages)
            {
                var imageId = Guid.NewGuid();
                if (item.Id == product.DefaultImage)
                {
                    product.DefaultImage = imageId;
                }              
                if (item.AttrValue1 == Guid.Empty && item.AttrValue2 == Guid.Empty && item.AttrValue3 == Guid.Empty)//如果三个库存属性为空则直接保存，不用与新的属性进行判断
                {
                    ProductImage image = new ProductImage();
                    image.Id = imageId;
                    image.AttrValue1 = item.AttrValue1;
                    image.AttrValue2 = item.AttrValue2;
                    image.AttrValue3 = item.AttrValue3;

                    if (item.ImageItems != null)
                    {
                        image.ImageItems = CopyProductImageItems(item.ImageItems.ToList(), imageId, product);
                    }
                    else
                    {
                        image.ImageItems = new List<ProductImageList>();
                    }
                    image.ProductId = product.Id;
                    image.Side = item.Side;
                    image.Type = item.Type;
                    insertProductImages.Add(image);
                }
                else//复制旧图
                {
                    if (item.Id == product.DefaultImage)
                    {
                        product.DefaultImage = imageId;
                    }
                    List<AttributeValueView> attrValues = new List<AttributeValueView>();
                    foreach (var attr in invAttrs)
                    {
                        if (attr.SubItems != null)
                        {
                            attrValues.AddRange(attr.SubItems);
                        }
                    }

                    if (CheckValueInCopyAttrValue(attrValues, item.AttrValue1, item.AttrValue2, item.AttrValue3))
                    {
                        ProductImage image = new ProductImage();
                        image.Id = imageId;
                        image.AttrValue1 = item.AttrValue1;
                        image.AttrValue2 = item.AttrValue2;
                        image.AttrValue3 = item.AttrValue3;

                        if (item.ImageItems != null)
                        {
                            image.ImageItems = CopyProductImageItems(item.ImageItems.ToList(), imageId, product);
                        }
                        else
                        {
                            image.ImageItems = new List<ProductImageList>();
                        }
                        image.ProductId = product.Id;
                        image.Side = item.Side;
                        image.Type = item.Type;
                        insertProductImages.Add(image);
                    }
                }              
            }

            if (insertProductImages.Any())
            {
                baseRepository.Insert(insertProductImages);
            }
        }

        private List<ProductImageList> CopyProductImageItems(List<ProductImageList> imageList, Guid newImageId, ProductEditModel product)
        {
            List<ProductImageList> result = new List<ProductImageList>();

            string relativePath = PathUtil.GetRelativePath(CurrentUser.MerchantId.ToString(), FileFolderEnum.Product) + "/" + product.Id;

            if (imageList.Any())
            {
                var newOriginalImageName = Guid.NewGuid() + Path.GetExtension(imageList[0].OriginalPath);
                result = imageList.Select(item => new ProductImageList
                {
                    Id = Guid.NewGuid(),
                    ImageID = newImageId,
                    Width = item.Width,
                    Length = item.Length,
                    OriginalPath = relativePath + "/" + newOriginalImageName,
                    Path = relativePath + "/" + Guid.NewGuid() + Path.GetExtension(item.Path),
                    Size = item.Size,
                    Type = item.Type,
                    IsActive = true
                }).ToList();
            }

            return result;
        }

        private bool CheckValueInCopyAttrValue(List<AttributeValueView> attrValues, Guid attrValue1, Guid attrValue2, Guid attrValue3)
        {
            var val1IsExists = attrValue1 == Guid.Empty ? true : attrValues.Where(p => p.Text == attrValue1.ToString()).Any();
            var val2IsExists = attrValue2 == Guid.Empty ? true : attrValues.Where(p => p.Text == attrValue2.ToString()).Any();
            var val3IsExists = attrValue3 == Guid.Empty ? true : attrValues.Where(p => p.Text == attrValue3.ToString()).Any();

            if (val1IsExists && val2IsExists && val3IsExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CopyProductRelationProduct(ProductEditModel product)
        {
            List<ProductRelatedItem> list = new List<ProductRelatedItem>();
            var oRelationProduct = baseRepository.GetList<ProductRelatedItem>(x => x.ProductId == product.OriginalId && x.IsActive && !x.IsDeleted).ToList();
            list = oRelationProduct.Select(item => new ProductRelatedItem
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                CreateDate = DateTime.Now,
                CreateBy = Guid.Parse(CurrentUser.UserId),
                ItemId = item.ItemId,
                ItemCode = item.ItemCode,
                Seq = item.Seq,
                IsActive = true
            }).ToList();

            if (list.Any())
            {
                baseRepository.Insert(list);
            }
        }

        public void InsertOrUpdateProductTranslation(Product dbProduct, ProductEditModel product, ActionTypeEnum type)
        {
            if (type == ActionTypeEnum.Add)
            {
                dbProduct.NameTransId = translationRepository.InsertMutiLanguage(product.ProductNames, TranslationType.Product);
                dbProduct.TitleTransId = translationRepository.InsertMutiLanguage(product.PageTitles, TranslationType.Product);
                dbProduct.IntroductionTransId = translationRepository.InsertMutiLanguage(product.ProductBriefs, TranslationType.Product);
                dbProduct.KeyWordTransId = translationRepository.InsertMutiLanguage(product.SeoKeywords, TranslationType.Product);
                dbProduct.SeoDescTransId = translationRepository.InsertMutiLanguage(product.SeoDescs, TranslationType.Product);
                //dbProduct.IngredientTransId = _translationRepository.InsertMutiLanguage(product.Ingredient, TranslationType.Product);
                //dbProduct.InstructionsTransId = _translationRepository.InsertMutiLanguage(product.Instructions, TranslationType.Product);

                product.NameTransId = dbProduct.NameTransId;
                product.TitleTransId = dbProduct.TitleTransId;
                product.IntroductionTransId = dbProduct.IntroductionTransId;
                product.KeyWordTransId = dbProduct.KeyWordTransId;
                product.SeoDescTransId = dbProduct.SeoDescTransId;
                //product.IngredientTransId = dbProduct.IngredientTransId;
                //product.InstructionsTransId = dbProduct.InstructionsTransId;
            }
            else
            {
                translationRepository.UpdateMutiLanguage(product.NameTransId, product.ProductNames, TranslationType.Product);
                translationRepository.UpdateMutiLanguage(product.TitleTransId, product.PageTitles, TranslationType.Product);
                translationRepository.UpdateMutiLanguage(product.IntroductionTransId, product.ProductBriefs, TranslationType.Product);
                translationRepository.UpdateMutiLanguage(product.KeyWordTransId, product.SeoKeywords, TranslationType.Product);
                translationRepository.UpdateMutiLanguage(product.SeoDescTransId, product.SeoDescs, TranslationType.Product);

                //dbProduct.IngredientTransId = _translationRepository.UpdateMutiLanguage(product.IngredientTransId, product.Ingredient, TranslationType.Product);
                //dbProduct.InstructionsTransId = _translationRepository.UpdateMutiLanguage(product.InstructionsTransId, product.Instructions, TranslationType.Product);
            }
        }

        private void InsertOrUpdateProductRefuseDelivery(ProductEditModel product)
        {
            if (product.CountryIds != null && product.CountryIds.Any())
            {
                var oldRefuses = baseRepository.GetList<ProductRefuseDelivery>(x => x.ProductId == product.Id && x.IsActive && !x.IsDeleted).ToList();
                baseRepository.Delete(oldRefuses);

                var newRefuse = product.CountryIds.Select(item => new ProductRefuseDelivery
                {
                    Id = Guid.NewGuid(),
                    CountryId = item,
                    ProductId = product.Id,
                    IsActive = true
                }).ToList();

                baseRepository.Insert(newRefuse);
            }

        }

        public void InsertProductStatistics(Product product)
        {
            var productStatistic = baseRepository.GetModel< ProductStatistics >(x=>x.Code == product.Code);
            if (productStatistic == null)
            {
                ProductStatistics model = new ProductStatistics();
                model.Id = Guid.NewGuid();
                model.Code = product.Code;
                model.ScoreNum = 0;
                model.Score = 0;
                model.InternalNameTransId = product.NameTransId;
                baseRepository.Insert(model);
            }

        }

        private void InsertOrUpdateProductDetail(ProductEditModel product, ActionTypeEnum type)
        {
            List<ProductDetail> details = new List<ProductDetail>();
            if (type == ActionTypeEnum.Add)
            {
                details = product.ProductDetail.Select(item => new ProductDetail
                {
                    Id = Guid.NewGuid(),
                    Lang = item.Language,
                    ProductId = product.Id,
                    Value = StringUtil.ConvertToHTML(StringUtil.FilterHTMLFunction(item.Desc)),
                    TransId = product.DetailTransId,
                    CreateBy = Guid.Parse(CurrentUser.UserId),
                }).ToList();
                baseRepository.Insert(details);
            }
            else
            {
                details = baseRepository.GetList<ProductDetail>(x => x.ProductId == product.Id && x.IsActive && !x.IsDeleted).ToList();
                if (details == null || !details.Any())
                {
                    details = product.ProductDetail.Select(item => new ProductDetail
                    {
                        Id = Guid.NewGuid(),
                        Lang = item.Language,
                        ProductId = product.Id,
                        Value = StringUtil.ConvertToHTML(StringUtil.FilterHTMLFunction(item.Desc)),
                        TransId = product.DetailTransId,
                        CreateBy = Guid.Parse(CurrentUser.UserId),
                    }).ToList();
                    baseRepository.Insert(details);
                }
                else
                {
                    foreach (var item in details)
                    {
                        item.Value = product.ProductDetail.FirstOrDefault(p => p.Language == item.Lang)?.Desc ?? "";
                        item.UpdateDate = DateTime.Now;
                        item.UpdateBy = Guid.Parse(CurrentUser.UserId);
                    }
                    baseRepository.Update(details);
                }
            }
        }

        public void InsertOrUpdateProductExtension(ProductEditModel product, ActionTypeEnum type)
        {
            if (type == ActionTypeEnum.Add)
            {
                ProductExtension extension = new ProductExtension();
                extension.Id = product.Id;
                extension.PermissionLevel = 1;//product.SpecifExtension.PermissionLevel;
                extension.IsOnSale = product.SpecifExtension.IsOnSale;
                extension.IsSaleOff = product.SpecifExtension.IsSaleOff;
                extension.MinPurQty = product.SpecifExtension.MinPurQty;
                extension.MaxPurQty = product.SpecifExtension.MaxPurQty;
                extension.YoutubeLink = product.SpecifExtension.YoutubeLink;
                extension.YoukuLink = product.SpecifExtension.YoukuLink;
                extension.ProductType = product.SpecifExtension.ProductType;
                extension.SafetyStock = product.SpecifExtension.SafetyStock;
                extension.IsLimit = product.SpecifExtension.IsLimit;
                extension.IsSalesReturn = product.SpecifExtension.NoRefund;
                extension.GS1Status = GS1Status.NONGS1;
                extension.HSCode = product.SpecifExtension.HSCode;
                extension.CreateBy = Guid.Parse(CurrentUser.UserId);
                baseRepository.Insert(extension);
            }
            else
            {
                var extension = baseRepository.GetModelById<ProductExtension>(product.Id);
                extension.PermissionLevel = 1;// product.SpecifExtension.PermissionLevel;
                extension.IsOnSale = product.SpecifExtension.IsOnSale;
                extension.IsSaleOff = product.SpecifExtension.IsSaleOff;
                extension.MinPurQty = product.SpecifExtension.MinPurQty;
                extension.MaxPurQty = product.SpecifExtension.MaxPurQty;
                extension.YoutubeLink = product.SpecifExtension.YoutubeLink;
                extension.YoukuLink = product.SpecifExtension.YoukuLink;
                extension.ProductType = product.SpecifExtension.ProductType;
                extension.IsLimit = product.SpecifExtension.IsLimit;
                extension.IsSalesReturn = product.SpecifExtension.NoRefund;
                extension.HSCode = product.SpecifExtension.HSCode;
                extension.UpdateDate = DateTime.Now;
                extension.UpdateBy = Guid.Parse(CurrentUser.UserId);
                baseRepository.Update(extension);
            }
        }

        public void InsertOrUpdateProductCommission(ProductEditModel product, ActionTypeEnum type)
        {
            if (product != null && product.CommissionConfig != null)
            {
                if (type == ActionTypeEnum.Add)
                {
                    ProductCommission prodCM = new ProductCommission()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        CMVal = product.CommissionConfig.CMVal,
                        CMRate = product.CommissionConfig.CMRate,
                        CreateBy =  Guid.Parse(CurrentUser.UserId),
                };

                    HandleProductCommissionValues(prodCM, product.CommissionConfig);

                    baseRepository.Insert(prodCM);
                }
                else
                {                    
                    ProductCommission prodCM = baseRepository.GetModelById<ProductCommission>(product.CommissionConfig.Id);
                    if (prodCM != null)
                    {
                        prodCM.ProductId = product.Id;
                        prodCM.CMVal = product.CommissionConfig.CMVal;
                        prodCM.CMRate = product.CommissionConfig.CMRate;
                        prodCM.UpdateBy = Guid.Parse(CurrentUser.UserId);
                        HandleProductCommissionValues(prodCM, product.CommissionConfig);

                        baseRepository.Update(prodCM);
                    }
                    else
                    {
                        InsertOrUpdateProductCommission(product, ActionTypeEnum.Add);
                    }
                }
            }
        }

        private void HandleProductCommissionValues(ProductCommission target, ProductCommissionView source)
        {
            if (target != null && source != null)
            {
                if ((int)source.CMCalType < 1)
                {
                    target.CMCalType = ProdCommissionType.MerchInheriting;
                    target.CMVal = null;
                    target.CMRate = null;
                }
                else
                {
                    target.CMCalType = source.CMCalType;
                    if (target.CMCalType == ProdCommissionType.FixedValue)
                    {
                        target.CMRate = null;
                    }
                    else if (target.CMCalType == ProdCommissionType.FixedRate)
                    {
                        target.CMVal = null;
                    }
                    else
                    {
                        target.CMVal = null;
                        target.CMRate = null;
                    }
                }
            }
        }

        public void InsertOrUpdateProductSpecification(ProductEditModel product, ActionTypeEnum type)
        {
            if (type == ActionTypeEnum.Add)
            {
                ProductSpecification specification = new ProductSpecification();

                specification.Id = product.Id;
                specification.NetWeight = product.Specification.NetWeight;
                specification.GrossWeight = product.Specification.GrossWeight;
                specification.WeightUnit = product.Specification.WeightUnit;
                specification.PackageDescription = product.Specification.PackageDescription;

                specification.Heigth = product.Specification.Heigth;
                specification.Width = product.Specification.Width;
                specification.Length = product.Specification.Length;
                specification.Unit = product.Specification.Unit;               
                specification.PackageHeigth = product.Specification.PackageHeight;
                specification.PackageWidth = product.Specification.PackageWidth;
                specification.PackageLength = product.Specification.PackageLength;
                specification.PackageUnit = product.Specification.PackageUnit;
                specification.CreateBy = Guid.Parse(CurrentUser.UserId);
                baseRepository.Insert(specification);
            }
            else
            {
                var specification = baseRepository.GetModelById<ProductSpecification>(product.Id);
                specification.NetWeight = product.Specification.NetWeight;
                specification.GrossWeight = product.Specification.GrossWeight;
                specification.WeightUnit = product.Specification.WeightUnit;
                specification.PackageDescription = product.Specification.PackageDescription;
                specification.Heigth = product.Specification.Heigth;
                specification.Width = product.Specification.Width;
                specification.Length = product.Specification.Length;
                specification.Unit = product.Specification.Unit;
                specification.PackageHeigth = product.Specification.PackageHeight;
                specification.PackageWidth = product.Specification.PackageWidth;
                specification.PackageLength = product.Specification.PackageLength;
                specification.PackageUnit = product.Specification.PackageUnit;
                specification.UpdateDate = DateTime.Now;
                specification.UpdateBy = Guid.Parse(CurrentUser.UserId);
                baseRepository.Update(specification);
            }
        }

        private void InsertOrUpdateInvProductAttribute(Product dbProduct, ProductEditModel product, ActionTypeEnum type)
        {
            if (type == ActionTypeEnum.Add)
            {
                List<ProductAttr> productAttrList = new List<ProductAttr>();

                //插入库存属性
                if (product.InveAttrList != null && product.InveAttrList.Any())
                {
                    List<ProductAttrValue> productAttrValueList = new List<ProductAttrValue>();
                    foreach (AttributeObjectView attr in product.InveAttrList)
                    {                       
                        var dbProductAttr = new ProductAttr();
                        dbProductAttr.Id = Guid.NewGuid();
                        dbProductAttr.AttrId = attr.Id;
                        dbProductAttr.ProductId = product.Id;
                        dbProductAttr.Seq = product.InveAttrList.IndexOf(attr) + 1;
                        dbProductAttr.IsInv = true;
                        dbProductAttr.CatalogID = product.Category;
                        dbProductAttr.IsActive = true;

                        var eachAttrValueList = attr.SubItems.Select(attrValue => new ProductAttrValue
                        {
                            Id = Guid.NewGuid(),
                            AttrValueId = Guid.Parse(attrValue.Text),
                            ProdAttrId = dbProductAttr.Id,
                            Seq = attr.SubItems.IndexOf(attrValue),
                            IsDeleted = false,
                            IsActive = true,
                            CreateBy = Guid.Parse(CurrentUser.UserId),
                            UpdateBy = Guid.Parse(CurrentUser.UserId),
                            AdditionalPrice = attrValue.Price
                        }).ToList();

                        productAttrList.Add(dbProductAttr);
                        productAttrValueList.AddRange(eachAttrValueList);
                    }

                    baseRepository.Insert(productAttrList);
                    baseRepository.Insert(productAttrValueList);
                }
                InsertProductSku(productAttrList, dbProduct.Code);
            }
            else//修改
            {              
                var isExistInvRec = CheckProductHasInventory(product.Id);
                if (!isExistInvRec)  //无库存记录
                {
                    //获取产品所有库存属性
                    var dbInvAttrs = baseRepository.GetList<ProductAttr>(x => x.ProductId == product.Id && x.IsInv && !x.IsDeleted).ToList();
                    //获取产品所有库存属性值
                    var dbProductAttrValues = baseRepository.GetList<ProductAttrValue>(x => dbInvAttrs.Select(s => s.Id).Contains(x.ProdAttrId) && !x.IsDeleted).ToList();

                    {
                        //var invAttrvalues = new List<AttributeValueView>();//界面所有的属性值
                        //var deleteInvAttrs = new List<ProductAttr>();//删除的库存属性
                        //var insertInvAttrValues = new List<ProductAttrValue>();//新增的库存属性值
                        //var deleteInvAttrValues = new List<ProductAttrValue>();//删除的库存属性值

                        //if (dbInvAttrs != null && dbInvAttrs.Any())
                        //{
                        //    foreach (var attr in product.InveAttrList)
                        //    {
                        //        invAttrvalues.AddRange(attr.SubItems);
                        //    }

                        //    foreach (var item in invAttrvalues)//获取需要新增的属性值
                        //    {
                        //        var attrId = Guid.Parse(item.Id);//ProductAttrs的Id
                        //        var attr = dbInvAttrs.FirstOrDefault(p => p.AttrId == attrId && p.IsDeleted == false);
                        //        if (attr != null)
                        //        {
                        //            var AttrValues = baseRepository.GetList<ProductAttrValue>(x => x.ProdAttrId == attr.Id);
                        //            var existAttrValue = AttrValues.FirstOrDefault(p => p.ProdAttrId == attr.Id && p.AttrValueId == Guid.Parse(item.Text));
                        //            if (existAttrValue == null)
                        //            {
                        //                ProductAttrValue attrValue = new ProductAttrValue();
                        //                attrValue.Id = Guid.NewGuid();

                        //                attrValue.AttrValueId = Guid.Parse(item.Text);
                        //                attrValue.ProdAttrId = attr.Id;
                        //                attrValue.Seq = 0;
                        //                attrValue.IsActive = true;
                        //                attrValue.AdditionalPrice = item.Price;
                        //                //attr.AttrValues.Add(attrValue);
                        //                insertInvAttrValues.Add(attrValue);
                        //            }
                        //            else if (existAttrValue != null && existAttrValue.IsDeleted == true)
                        //            {
                        //                existAttrValue.AdditionalPrice = item.Price;
                        //                existAttrValue.IsDeleted = false;
                        //                insertInvAttrValues.Add(existAttrValue);
                        //            }
                        //            else
                        //            {
                        //                existAttrValue.AdditionalPrice = item.Price;
                        //            }
                        //        }

                        //    }

                        //    foreach (var item in dbInvAttrs)//获取需要删除的属性值
                        //    {
                        //        var AttrValues = baseRepository.GetList<ProductAttrValue>(x => x.ProdAttrId == item.Id);
                        //        foreach (var attrValue in AttrValues)
                        //        {
                        //            var existAttrValue = invAttrvalues.FirstOrDefault(p => p.Text == attrValue.AttrValueId.ToString());
                        //            if (existAttrValue == null)
                        //            {
                        //                attrValue.AdditionalPrice = 0;
                        //                attrValue.IsDeleted = true;
                        //                deleteInvAttrValues.Add(attrValue);
                        //            }

                        //        }
                        //    }

                        //    if (dbInvAttrs[0].CatalogID == product.Category)//判断属性的catalogId与传入的catalogID是否相同
                        //    {                      
                        //        if (insertInvAttrValues.Any() || deleteInvAttrValues.Any())//在没有库存的情况下，只要有增删属性值，则从新生成SKU
                        //        {                               
                        //            deleteInvAttrValues = new List<ProductAttrValue>();
                        //            foreach (var attr in dbInvAttrs)
                        //            {
                        //                attr.IsDeleted = true;
                        //                deleteInvAttrs.Add(attr);
                        //                var AttrValues = baseRepository.GetList<ProductAttrValue>(x => x.ProdAttrId == attr.Id).ToList();
                        //                foreach (var attrValue in AttrValues)
                        //                {
                        //                    attrValue.IsDeleted = true;
                        //                }
                        //                deleteInvAttrValues.AddRange(AttrValues);
                        //            }

                        //            //_productImageBLL.DeleteImageByProdId(product.Id);
                        //            baseRepository.Update(deleteInvAttrs);//删除库存属性
                        //            DeleteProductSku(deleteInvAttrValues, product);
                        //            InsertOrUpdateInvProductAttribute(dbProduct, product, ActionTypeEnum.Add);//重新添加产品的库存属性、库存属性值、SKU

                        //        }
                        //    }
                        //    else
                        //    {
                        //        deleteInvAttrValues = new List<ProductAttrValue>();
                        //        foreach (var attr in dbInvAttrs)
                        //        {
                        //            attr.IsDeleted = true;
                        //            deleteInvAttrs.Add(attr);
                        //            var AttrValues = baseRepository.GetList<ProductAttrValue>(x => x.ProdAttrId == attr.Id);
                        //            foreach (var attrValue in AttrValues)
                        //            {
                        //                attrValue.IsDeleted = true;
                        //                deleteInvAttrValues.Add(attrValue);
                        //            }
                        //        }

                        //        //_productImageBLL.DeleteImageByProdId(product.Id);
                        //        baseRepository.Update(deleteInvAttrs);//删除库存属性
                        //        DeleteProductSku(deleteInvAttrValues, product);
                        //        InsertOrUpdateInvProductAttribute(dbProduct, product, ActionTypeEnum.Add);//重新添加产品的库存属性、库存属性值、SKU
                        //    }
                        //}
                    }

                    dbProduct.CatalogId = product.Category;

                    foreach (var item in dbInvAttrs)
                    {
                        item.IsDeleted = true;
                        item.UpdateDate = DateTime.Now;
                    }

                    foreach (var item in dbProductAttrValues)
                    {
                        item.IsDeleted = true;
                        item.UpdateDate = DateTime.Now;
                    }

                    baseRepository.Update(dbProduct);
                    baseRepository.Update(dbInvAttrs);
                    baseRepository.Update(dbProductAttrValues);

                    DeleteProductSku(dbProductAttrValues, product);
                    InsertOrUpdateInvProductAttribute(dbProduct, product, ActionTypeEnum.Add);
                }
            }
        }

        private void InsertOrUpdateNonInvProductAttribute(Product dbProduct, ProductEditModel product, ActionTypeEnum type)
        {
            if (type == ActionTypeEnum.Add)
            {
                List<ProductAttr> productAttrList = new List<ProductAttr>();
                //插入非库存属性
                if (product.NonInveAttrList != null && product.NonInveAttrList.Any())
                {
                    List<ProductAttrValue> productAttrValueList = new List<ProductAttrValue>();
                    foreach (AttributeObjectView attr in product.NonInveAttrList)
                    {
                        var dbProductAttr = new ProductAttr();
                        dbProductAttr.Id = Guid.NewGuid();
                        dbProductAttr.AttrId = attr.Id;
                        dbProductAttr.ProductId = product.Id;
                        dbProductAttr.IsInv = false;
                        dbProductAttr.CatalogID = product.Category;
                        dbProductAttr.Seq = product.NonInveAttrList.IndexOf(attr) + 1;

                        var eachAttrValueList = attr.SubItems.Select(attrValue => new ProductAttrValue
                        {
                            Id = Guid.NewGuid(),
                            AttrValueId = Guid.Parse(attrValue.Text),
                            ProdAttrId = dbProductAttr.Id,
                            Seq = attr.SubItems.IndexOf(attrValue),
                            IsDeleted = false,
                            IsActive = true,
                            CreateBy = Guid.Parse(CurrentUser.UserId),
                            UpdateBy = Guid.Parse(CurrentUser.UserId),
                            AdditionalPrice = attrValue.Price
                        }).ToList();

                        productAttrValueList.AddRange(eachAttrValueList);                      
                        productAttrList.Add(dbProductAttr);
                    }

                    baseRepository.Insert(productAttrList);
                    baseRepository.Insert(productAttrValueList);
                }
            }
            else
            {

                //获取产品所有非库存属性
                var dbNonInvAttrs = baseRepository.GetList<ProductAttr>(x => x.ProductId == product.Id && !x.IsInv && !x.IsDeleted).ToList();
                //获取产品所有非库存属性值
                var dbProductAttrValues = baseRepository.GetList<ProductAttrValue>(x => dbNonInvAttrs.Select(s => s.Id).Contains(x.ProdAttrId) && !x.IsDeleted).ToList();

                {
                    //var nonInvAttrvalues = new List<AttributeValueView>();//界面所有的属性值
                    //var deleteNonInvAttrs = new List<ProductAttr>();//删除的非库存属性
                    //var insertNonInvAttrValues = new List<ProductAttrValue>();//新增的非库存属性值
                    //var deleteNonInvAttrValues = new List<ProductAttrValue>();//删除的非库存属性值

                    //if (dbNonInvAttrs != null && dbNonInvAttrs.Any())
                    //{
                    //    foreach (var attr in product.NonInveAttrList)
                    //    {
                    //        nonInvAttrvalues.AddRange(attr.SubItems);
                    //    }

                    //    foreach (var item in nonInvAttrvalues)//获取需要新增的属性值
                    //    {
                    //        var attr = dbNonInvAttrs.FirstOrDefault(p => p.AttrId == Guid.Parse(item.Id));
                    //        if (attr != null)
                    //        {
                    //            var AttrValues = baseRepository.GetList<ProductAttrValue>(x => x.ProdAttrId == attr.Id);
                    //            var existAttrValue = AttrValues.FirstOrDefault(p => p.AttrValueId == Guid.Parse(item.Text));
                    //            if (existAttrValue == null)
                    //            {
                    //                ProductAttrValue attrValue = new ProductAttrValue();
                    //                attrValue.Id = Guid.NewGuid();
                    //                attrValue.AttrValueId = Guid.Parse(item.Text);
                    //                attrValue.ProdAttrId = attr.Id;
                    //                attrValue.Seq = 0;
                    //                attrValue.AdditionalPrice = item.Price;
                    //                //attr.AttrValues.Add(attrValue);
                    //                insertNonInvAttrValues.Add(attrValue);
                    //            }
                    //            else if (existAttrValue != null && existAttrValue.IsDeleted == true)
                    //            {
                    //                existAttrValue.IsDeleted = false;
                    //                insertNonInvAttrValues.Add(existAttrValue);
                    //            }
                    //        }

                    //    }

                    //    foreach (var item in dbNonInvAttrs)//获取需要删除的属性值
                    //    {
                    //        var AttrValues = baseRepository.GetList<ProductAttrValue>(x => x.AttrValueId == item.Id);
                    //        foreach (var attrValue in AttrValues)
                    //        {
                    //            if (attrValue.IsDeleted == false)
                    //            {
                    //                var existAttrValue = nonInvAttrvalues.FirstOrDefault(p => p.Text == attrValue.AttrValueId.ToString());
                    //                if (existAttrValue == null)
                    //                {
                    //                    attrValue.IsDeleted = true;
                    //                    deleteNonInvAttrValues.Add(attrValue);
                    //                }
                    //            }
                    //        }
                    //    }

                    //    if (product.IsExistInvRec)//判断是否有库存，有库存只对属性值进行删改
                    //    {
                    //        baseRepository.Update(dbNonInvAttrs);
                    //        //baseRepository.Update(deleteNonInvAttrValues);  //??
                    //    }
                    //    else
                    //    {
                    //        if (dbNonInvAttrs[0].CatalogID == product.Category)         //判断属性的catalogId与传入的catalogID是否相同
                    //        {
                    //            if (nonInvAttrvalues.Any())
                    //            {
                    //                baseRepository.Update(dbNonInvAttrs);
                    //                baseRepository.Insert(insertNonInvAttrValues);
                    //            }
                    //            else
                    //            {
                    //                deleteNonInvAttrValues = new List<ProductAttrValue>();
                    //                foreach (var attr in dbNonInvAttrs)
                    //                {
                    //                    attr.IsDeleted = true;
                    //                    deleteNonInvAttrs.Add(attr);
                    //                    var AttrValues = baseRepository.GetList<ProductAttrValue>(x => x.ProdAttrId == attr.Id);
                    //                    foreach (var attrValue in AttrValues)
                    //                    {
                    //                        attrValue.IsDeleted = true;
                    //                        deleteNonInvAttrValues.Add(attrValue);
                    //                    }
                    //                }
                    //                baseRepository.Update(dbNonInvAttrs);//删除库存属性  
                    //                InsertOrUpdateNonInvProductAttribute(dbProduct, product, ActionTypeEnum.Add);//重新添加产品的库存属性、库存属性
                    //            }

                    //        }
                    //        else    //如果重选Catalog
                    //        {
                    //            deleteNonInvAttrValues = new List<ProductAttrValue>();
                    //            foreach (var attr in dbNonInvAttrs)
                    //            {
                    //                attr.IsDeleted = true;
                    //                deleteNonInvAttrs.Add(attr);
                    //                var AttrValues = baseRepository.GetList<ProductAttrValue>(x => x.ProdAttrId == attr.Id);
                    //                foreach (var attrValue in AttrValues)
                    //                {
                    //                    attrValue.IsDeleted = true;
                    //                    deleteNonInvAttrValues.Add(attrValue);
                    //                }
                    //            }
                    //            baseRepository.Update(dbNonInvAttrs);//删除库存属性

                    //            InsertOrUpdateNonInvProductAttribute(dbProduct, product, ActionTypeEnum.Add);//重新添加产品的库存属性、库存属性
                    //        }
                    //    }
                    //}
                }

                dbProduct.CatalogId = product.Category;

                foreach (var item in dbNonInvAttrs)
                {
                    item.IsDeleted = true;
                    item.UpdateDate = DateTime.Now;
                }

                foreach (var item in dbProductAttrValues)
                {
                    item.IsDeleted = true;
                    item.UpdateDate = DateTime.Now;
                }

                baseRepository.Update(dbProduct);
                baseRepository.Update(dbNonInvAttrs);
                baseRepository.Update(dbProductAttrValues);

                InsertOrUpdateNonInvProductAttribute(dbProduct, product, ActionTypeEnum.Add);
            }
        }

        private void InsertProductSku(List<ProductAttr> attrs, string code)
        {
            var skus = productSkuRepository.GenProduckSku(attrs, code);           
            foreach (var item in skus)
            {
                var dbSku = productSkuRepository.GetSkuByAttrValueId(item.ProductCode, item.AttrValue1, item.AttrValue2, item.AttrValue3);
                if (dbSku == null)
                {
                    item.CreateBy =  Guid.Parse(CurrentUser.UserId);
                    baseRepository.Insert(item);
                }
                else
                {
                    dbSku.IsDeleted = false;
                    dbSku.IsActive = true;
                    dbSku.UpdateDate = DateTime.Now;
                    dbSku.UpdateBy = Guid.Parse(CurrentUser.UserId);
                    baseRepository.Update(dbSku);
                }

            }
        }

        private void DeleteProductSku(List<ProductAttrValue> deleteList, ProductEditModel product)
        {
            //如果需要删除属性值，则要删除有该属性值的SKU
            if (deleteList != null && deleteList.Any())
            {
                var deleteSkus = new List<ProductSku>();//删除的SKU记录
                foreach (var item in deleteList)
                {
                    var productSKUs = productSkuRepository.GetSkuByAttrValueId(product.Code, item.AttrValueId);
                    foreach (var sku in productSKUs)
                    {
                        sku.IsDeleted = true;
                        //deleteSkus.Add(sku);
                    }
                    deleteSkus.AddRange(productSKUs);
                }
                baseRepository.Update(deleteSkus);
            }
        }

        private void InsertOrUpdateFreeChargeProduct(ProductEditModel product)
        {
            Merchant merchant = baseRepository.GetModel<Merchant>(x => x.Id == product.MerchantId);
            var freeMethods = baseRepository.GetList<MerchantFreeCharge>(x => x.ProductCode == product.Code).ToList();
            if (product.SpecifExtension.ProductType == ProductType.FreeShip)
            {
                var activeShipMethods = merchantShipMethodMappingRepository.GetShipMethidByMerchantId(product.MerchantId).Where(p => p.IsEffect).ToList();
                List<MerchantFreeCharge> productFreeCharges = activeShipMethods.Select(item => new MerchantFreeCharge
                {
                    Id = Guid.NewGuid(),
                    MerchantId = product.MerchantId,
                    ProductCode = product.Code,
                    ShipCode = item.ShipCode,

                }).ToList();

                baseRepository.Delete(freeMethods);
                baseRepository.Insert(productFreeCharges);
            }
            else
            {
                if (merchant.IsExternal)
                {
                    baseRepository.Delete(freeMethods);
                }
                else
                {
                    freeMethods = freeMethods.Where(p => p.ShipCode != "CC").ToList();
                    baseRepository.Delete(freeMethods);
                }
            }
        }

        /// <summary>
        /// 如果在时段价格内修改过则备注ProductPriceHour一下 
        /// </summary>
        /// <param name="product"></param>
        void UpdateProductHourPriceRemak(ProductEditModel product)
        {
            if (product.IsPriceRemark)
            {
                var ppHour = baseRepository.GetModel<ProductPriceHour>(d => !d.IsDeleted && d.ProductCode == product.Code && d.MerchantId == product.MerchantId
                                        && d.IsTimeStatus && DateTime.Now >= d.BeginTime && DateTime.Now <= d.EndTime);
                if (ppHour != null)
                {
                    ppHour.ReMark = product.IsPriceRemark.ToString();
                    ppHour.UpdateDate = DateTime.Now;
                    baseRepository.Update(ppHour);
                }
            }
        }

        private void GenSkuImagePath(ProductEditModel product, List<KeyValue> dbImagePaths, List<ImageSize> imageSizes)
        {
            string defaultImage = PathUtil.GetDefaultImagePath();// .GetPhysicalPath(CurrentUser.ClientId, product.MerchantId, FileFolderEnum.TempPath);
            string targetPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], product.MerchantId.ToString(), FileFolderEnum.Product) + "\\" + product.Id;
            string relativePath = PathUtil.GetRelativePath(product.MerchantId.ToString(), FileFolderEnum.Product) + "/" + product.Id;

            defaultImage = Path.Combine(hostEnvironment.ContentRootPath, defaultImage);

            if (File.Exists(defaultImage))
            {
                foreach (var item in imageSizes)
                {
                    var imageItemId = Guid.NewGuid();
                    dbImagePaths.Add(new KeyValue { Id = imageItemId.ToString(), Text = relativePath + "/" + imageItemId + Path.GetExtension(defaultImage) });
                }
            }
        }

        public ProductSummary GenProductSummary(Product product)
        {
            return GenProductSummary(product, Guid.Empty);
        }

        public ProductSummary GenProductSummary(Product product, Guid skuId)
        {
            ProductSummary summary = AutoMapperExt.MapTo<ProductSummary>(product);
            var statistic = baseRepository.GetModel<ProductStatistics>(x => x.Code == product.Code);
            var catalogs = baseRepository.GetModelById<ProductCatalog>(product.CatalogId);
            var mchInfo = baseRepository.GetModelById<Merchant>(product.MerchantId);
            var prodSepcification = baseRepository.GetModel<ProductSpecification>(x=>x.Id== product.Id);
            var prodExtension = baseRepository.GetModel<ProductExtension>(x => x.Id == product.Id);

            summary.CatalogPath = "";
            summary.CatalogName = translationRepository.GetDescForLang((catalogs?.NameTransId ?? Guid.Empty), CurrentUser.Lang);
            summary.Code = product.Code;
            summary.Currency = currencyBLL.GetSimpleCurrency(product.CurrencyCode);
            summary.Imgs = GetProductImages(product.Id);
            summary.Name = translationRepository.GetDescForLang(product.NameTransId, CurrentUser.Lang);
            summary.Introduction = translationRepository.GetDescForLang(product.IntroductionTransId,CurrentUser.Lang);
            summary.OriginalPrice = product.OriginalPrice + product.MarkUpPrice;
            summary.SalePrice = product.SalePrice + product.MarkUpPrice;
            summary.ProductId = product.Id;
            summary.ApproveType = product.Status;          
            summary.MerchantNo = mchInfo?.MerchNo ?? string.Empty;
            summary.MerchantNameId = mchInfo.NameTransId;
            summary.MerchantName = translationRepository.GetDescForLang(mchInfo.NameTransId,CurrentUser.Lang);
            summary.IsGS1 = MerchantType.GS1  ==mchInfo?.MerchantType ? true : false;          
            summary.PurchaseCounter = statistic == null ? 0 : statistic.PurchaseCounter;
            summary.VisitCounter = statistic == null ? 0 : statistic.VisitCounter;

            summary.Score = statistic == null ? 0M : NumberUtil.ConvertToRounded(statistic.Score);
            if (summary.Score < 1)
            {
                summary.Score = 0;
            }
            summary.WeightUnit = prodSepcification?.WeightUnit ?? 0;
            summary.GrossWeight = prodSepcification?.GrossWeight ?? 0;
            summary.IconType = prodExtension?.ProductType ?? ProductType.Basic;
            summary.IconRType = prodExtension?.ProductType;
            summary.IconType = prodExtension?.ProductType ?? ProductType.Basic;
            summary.IsLimit = prodExtension?.IsLimit ?? false;

        
            
            //var addPrices = new List<ProductAtrtValueModel>();
            //if (skuId != Guid.Empty)
            //{
            //    #region 生成產品屬性

            //    var productInvAttrs = product.Attrs.Where(p => p.IsInv == true).OrderBy(o => o.Seq).ToList();

            //    var skuInfo = _productSkuRepository.GetByKey(skuId);

            //    //var skuAttrValues1 = productInvAttrs.FirstOrDefault(p => p.AttrId == skuInfo.Attr1)?.AttrValues;
            //    //var skuAttrValues2 = productInvAttrs.FirstOrDefault(p => p.AttrId == skuInfo.Attr2)?.AttrValues;
            //    //var skuAttrValues3 = productInvAttrs.FirstOrDefault(p => p.AttrId == skuInfo.Attr3)?.AttrValues;

            //    // var AttrValueLst = UnitOfWork.DataContext.ProductAttrs.Where(x => x.IsInv == true && x.ProductId == product.Id)
            //    var AttrValueLst = productInvAttrs.Join(UnitOfWork.DataContext.ProductAttrValues,
            //                                    pa => pa.Id, pav => pav.ProdAttrId, (pa, pav) => pav).ToList();

            //    var productAttrValue1 = AttrValueLst.FirstOrDefault(f => f.AttrValueId == skuInfo.AttrValue1);
            //    var productAttrValue2 = AttrValueLst.FirstOrDefault(f => f.AttrValueId == skuInfo.AttrValue2);
            //    var productAttrValue3 = AttrValueLst.FirstOrDefault(f => f.AttrValueId == skuInfo.AttrValue3);

            //    var attribute = UnitOfWork.DataContext.ProductAttributes.AsNoTracking()
            //                            .Where(a => a.Id == skuInfo.Attr1 || a.Id == skuInfo.Attr2 || a.Id == skuInfo.Attr3).ToList();

            //    var attributeValues = UnitOfWork.DataContext.ProductAttributeValues.AsNoTracking()
            //                           .Where(a => a.Id == skuInfo.AttrValue1 || a.Id == skuInfo.AttrValue2 || a.Id == skuInfo.AttrValue3).ToList();

            //    if (productAttrValue1 != null)
            //    {
            //        ProductAtrtValueModel model = GenProductAtrtValueModel(productAttrValue1, skuInfo.Attr1, attribute, attributeValues);
            //        addPrices.Add(model);
            //    }
            //    if (productAttrValue2 != null)
            //    {
            //        ProductAtrtValueModel model = GenProductAtrtValueModel(productAttrValue2, skuInfo.Attr2, attribute, attributeValues);
            //        addPrices.Add(model);
            //    }
            //    if (productAttrValue3 != null)
            //    {
            //        ProductAtrtValueModel model = GenProductAtrtValueModel(productAttrValue3, skuInfo.Attr3, attribute, attributeValues);
            //        addPrices.Add(model);
            //    }
            //}

            //#endregion

            //summary.AttrValues = addPrices;
            //summary.TotalPrice = summary.SalePrice + addPrices.Sum(s => s.AddPrice);
            //if (CurrentUser != null)
            //{
            //    if (CurrentUser.IsLogin)
            //    {
            //        var favoriteList = MemberFavoriteRepos.Entities.FirstOrDefault(d => d.MemberId == CurrentUser.Id && d.ProductCode == product.Code && d.IsActive == true && d.IsDeleted == false);
            //        if (favoriteList != null)
            //        {
            //            summary.IsFavorite = true;
            //        }
            //    }

            //    summary.IconRUrl = PathUtil.GetProductIconUrl(summary.IconRType, CurrentUser.ComeFrom, CurrentUser.Language);
            //    if (summary.IsGS1)
            //    {
            //        summary.GS1Url = PathUtil.GetProductIconUrl(ProductType.GS1, CurrentUser.ComeFrom, CurrentUser.Language);
            //    }
            //    else
            //    {
            //        summary.GS1Url = PathUtil.GetProductIconUrl(ProductType.Basic, CurrentUser.ComeFrom, CurrentUser.Language);
            //    }
            //    if (product.DetailDescriptions?.Count > 0)
            //    {
            //        summary.DescriptionDetail = product.DetailDescriptions.FirstOrDefault(x => x.Lang == CurrentUser.Language)?.Value ?? string.Empty;
            //    }
            //    if (!IsMatchBaseCurrency(CurrentUser.Currency))
            //    {
            //        summary.Currency2 = CurrentUser.Currency;
            //        summary.SalePrice2 = MoneyConversion(product.SalePrice);
            //        summary.OriginalPrice2 = MoneyConversion(product.OriginalPrice);
            //    }
            //}

            //summary.PromotionRuleTitle = PromotionRuleRepository.GetPromotionRuleTitle(product.Code);
            //summary.IsSalesReturn = product.ProductExtension?.IsSalesReturn ?? false;

            if (!string.IsNullOrEmpty(product.Remark))
            {
            
                    var saleTimes = product.Remark.Trim().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (saleTimes.Length >= 2)
                    {
                        var time = DateUtil.ConvertoDateTime(saleTimes[1], "yyyy-MM-dd HH:mm:00");
                        summary.SaleTime = time;
                    }
                    else
                    {
                        summary.SaleTime = null;
                    }
            }
            else
            {
                summary.SaleTime = null;
            }
            return summary;
        }
    }
}
