using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
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
        public PageData<ProductSummary> SearchData([FromForm]ProdSearchCond condition)
        {
            PageData<ProductSummary> result = productBLL.SearchBackEndProductSummary(condition);

            return result;
        }
    }

}
