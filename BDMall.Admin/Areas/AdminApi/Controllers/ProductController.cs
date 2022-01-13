using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [AdminApiAuthorize(Module = ModuleConst.ProductModule)]
    [ApiController]
    public class ProductController : BaseApiController
    {
        IProductBLL productBLL;
        IAttributeBLL attributeBLL;

        public ProductController(IComponentContext services) : base(services)
        {
            productBLL = Services.Resolve<IProductBLL>();
            attributeBLL = Services.Resolve<IAttributeBLL>();
        }

        [HttpGet]
        public Dictionary<string, ClickRateSummaryView> GetClickRateSummary(int topMonthQty, int topWeekQty, int topDayQty)
        {
            var data = productBLL.GetClickRateView(topMonthQty, topWeekQty, topDayQty);
            return data;
        }

        [HttpGet]
        public Dictionary<string, ClickRateSummaryView> GetSearchRateSummary(int topMonthQty, int topWeekQty, int topDayQty)
        {
            var data = productBLL.GetSearchRateView(topMonthQty, topWeekQty, topDayQty);
            return data;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Search })]
        public SystemResult GetProductCondition()
        {
            SystemResult result = new SystemResult();           
            var keyWordType = GetKeyWordType();
            var attribute = attributeBLL.GetInveAttribute(); //GetAllAttribute(lang);
            var attributeValue = attributeBLL.GetInveAttributeValueSummary(); //GetAllAttribute(lang);
            var permissions = GetPermission();
            var sortedKey = GetSortedKey();
            var sortedKeyType = GetSortedType();
            //var isActive = GetIsActive();
            var approveStatus = GetApproveStatus();

            var  obj = new
            {
                //Skins = sizes,
                //Virtues = functions,
                //ProductTypes = productType,
                KeyWordTypes = keyWordType,
                Attributes = attribute,
                AttributeValues = attributeValue,
                Permissions = permissions,
                SortedKeys = sortedKey,
                SortedKeyTypes = sortedKeyType,
                //IsActive = isActive,
                ApproveStatus = approveStatus,
            };

            result.Succeeded = true;
            result.ReturnValue = obj;

            return result;
        }

        /// <summary>
        /// 获取Product搜寻页面的keyWordType
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Search })]
        public List<KeyValue> GetKeyWordType()
        {          
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CultureHelper.GetSupportCulture(CurrentUser.Lang.ToString()));
            Resources.Value.Culture = Thread.CurrentThread.CurrentCulture;
            List<KeyValue> list = new List<KeyValue>();
            list.Add(new KeyValue { Id = "0", Text = Resources.Value.ProductSearchTypeAllWords });
            list.Add(new KeyValue { Id = "1", Text = Resources.Value.ProductSearchTypeAnyWord });

            return list;
        }

        /// <summary>
        /// 获取Permission信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValue> GetPermission()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CultureHelper.GetSupportCulture(CurrentUser.Lang.ToString()));
            Resources.Value.Culture = Thread.CurrentThread.CurrentCulture;
            List<KeyValue> list = new List<KeyValue>();
            list.Add(new KeyValue { Id = "1", Text = Resources.Value.PermissionvVewablebypublic });
            list.Add(new KeyValue { Id = "2", Text = Resources.Value.PermissionvViewableafterlogin });
            list.Add(new KeyValue { Id = "3", Text = Resources.Value.PermissionvForinternaluseonly });

            return list;
        }

        /// <summary>
        /// 获取排序信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValue> GetSortedKey()
        {            
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CultureHelper.GetSupportCulture(CurrentUser.Lang.ToString()));
            Resources.Value.Culture = Thread.CurrentThread.CurrentCulture;
            List<KeyValue> list = new List<KeyValue>();
            list.Add(new KeyValue { Id = "Code", Text = Resources.Value.OrderByProductCode });
            list.Add(new KeyValue { Id = "Name", Text = Resources.Value.OrderByName });
            list.Add(new KeyValue { Id = "ListPrice", Text = Resources.Value.OrderByPrice });

            return list;
        }

        /// <summary>
        /// 获取排序类型、升序、降序
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValue> GetSortedType()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CultureHelper.GetSupportCulture(CurrentUser.Lang.ToString()));
            Resources.Value.Culture = Thread.CurrentThread.CurrentCulture;
            List<KeyValue> list = new List<KeyValue>();
            list.Add(new KeyValue { Id = "OrderBy", Text = Resources.Value.OrderByTypeASC });
            list.Add(new KeyValue { Id = "OrderByDescending", Text = Resources.Value.OrderByTypeDSC });

            return list;
        }

        /// <summary>
        /// 獲取是否審批
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValue> GetApproveStatus()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CultureHelper.GetSupportCulture(CurrentUser.Lang.ToString()));
            Resources.Value.Culture = Thread.CurrentThread.CurrentCulture;
            List<KeyValue> list = new List<KeyValue>();
            list.Add(new KeyValue { Id = "0", Text = Resources.Value.Editing });
            list.Add(new KeyValue { Id = "1", Text = Resources.Value.WaitingApprove });
            list.Add(new KeyValue { Id = "2", Text = Resources.Value.Reject });
            list.Add(new KeyValue { Id = "3", Text = Resources.Value.Pass });
            list.Add(new KeyValue { Id = "4", Text = Resources.Value.OnSale });
            list.Add(new KeyValue { Id = "5", Text = Resources.Value.ManualOffSale });
            list.Add(new KeyValue { Id = "6", Text = Resources.Value.AutoOffSale });

            return list;
        }

        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Search })]
        public PageData<ProductSummary> SearchData([FromBody]ProdSearchCond condition)
        {
            PageData<ProductSummary> result = productBLL.SearchBackEndProductSummary(condition);

            return result;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_View })]
        public ProductEditModel GetById(Guid id)
        {
            ProductEditModel product = new ProductEditModel();
            product = productBLL.GetProductInfo(id);           
            return product;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Edit })]
        public ProductEditModel GetCopyProduct(Guid id)
        {
            ProductEditModel product = new ProductEditModel();
            product = productBLL.GetProductInfo(id);
            product.Code = "";          
            product.OriginalId = Guid.Empty;
            product.IsExistInvRec = false;
            product.IsApprove = false;
            product.SaleTime = null;
            product.Action = ActionTypeEnum.Copy.ToString();
            product.SalePrice = product.TimePrice;

            return product;
        }

        /// <summary>
        /// 检查是否有时段价格
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public SystemResult CheckTimePriceByCode(string code, Guid MerchantId)
        {
            var result = productBLL.CheckTimePriceByCode(code, MerchantId);
            return result;
        }

        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Edit })]
        public async Task<SystemResult> Save([FromForm]ProductEditModel product)
        {
            SystemResult result = new SystemResult();
            product.Validate();

            var productInfo = productBLL.SaveProduct(product);
          
            var prodcutModel = AutoMapperExt.MapTo<ProductEditModel>(productInfo);
            if (product.Action == ActionTypeEnum.NewVer.ToString() || product.Action == ActionTypeEnum.Copy.ToString())       
                await productBLL.CopyProductImageToPath(prodcutModel);
            
            if (product.Action == ActionTypeEnum.Add.ToString())          //新建时创建一个默认图片
                await productBLL.CreateDefaultImage(prodcutModel);

            //更新商品缓存
            await productBLL.UpdateCache(productInfo.Code, ProdAction.Apporve);

            result.Succeeded = true;
            result.Message = "";
            result.ReturnValue = productInfo.Id;
            return result;
        }

        [HttpPost]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Edit })]
        public PageData<ProductSummary> SearchRelatedProduct(RelatedProductCond condition)
        {
            PageData<ProductSummary> list = list = productBLL.SearchRelatedProduct(condition);         
            return list;
        }

        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Edit })]
        public List<ProductSummary> GetRelatedProduct(Guid id)
        {
            List<ProductSummary> list = productBLL.GetRelatedProduct(id);          
            return list;
        }

        /// <summary>
        /// 為產品添加關聯產品
        /// </summary>
        /// <param name="OriginalSku"></param>
        /// <param name="products">1,2,3</param>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Edit })]
        public SystemResult AddRelatedProduct(Guid OriginalId, string products)
        {
            SystemResult result = new SystemResult();

            var lisrProducts = products.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            productBLL.AddRelatedProduct(lisrProducts, OriginalId);

            result.Succeeded = true;
            result.Message = "";
            return result;
        }

        /// <summary>
        /// 刪除產品的關聯產品
        /// </summary>
        /// <param name="OriginalSku"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Edit })]
        public SystemResult DeleteRelatedProduct(Guid prodID, string products)
        {
            SystemResult result = new SystemResult();

            var lisrProducts = products.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            productBLL.DeleteRelatedProduct(prodID, lisrProducts);
            result.Succeeded = true;
            result.Message = "";
            return result;
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="prodIDs"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Del })]
        public async Task<SystemResult> Delete(string prodIDs)
        {
            SystemResult result = new SystemResult();
            var list = prodIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            await productBLL.ProductLogicalDelete(list);
            result.Succeeded = true;
            result.Message = "";
            return result;
        }

        /// <summary>
        /// 设置为上架
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Edit })]
        public async Task<SystemResult> ActiveProducts(string ids)
        {
            SystemResult result = new SystemResult();
            var idArr = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            await productBLL.ActiveProducts(idArr);
            result.Succeeded = true;
            result.Message = "";

            return result;
        }

        /// <summary>
        /// 设置为下架
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Edit })]
        public async Task<SystemResult> DisActiveProducts(string ids)
        {
            SystemResult result = new SystemResult();

            var idArr = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            await productBLL.DisActiveProducts(idArr);
            result.Succeeded = true;
            result.Message = "";
           
            return result;
        }

        /// <summary>
        /// 将产品状品改为待审批
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Edit })]
        public SystemResult ApplyApprove(Guid id)
        {
             return productBLL.ApplyApprove(id); 
        }

        /// <summary>
        /// 拒絕通過產品
        /// </summary>
        /// <param name="prodID">產品ID</param>
        /// <param name="reason">原因</param>
        [HttpGet]
        [AdminApiAuthorize(Module = ModuleConst.ProductModule, Function = new string[] { FunctionConst.Prod_Approve })]
        public async Task<SystemResult> TurndownProduct(Guid prodID, string reason)
        {
            SystemResult result = new SystemResult();
            await productBLL.TurndownProduct(prodID, reason);
            result.Succeeded = true;
            result.Message = "";

            return result;
        }

    }

}
