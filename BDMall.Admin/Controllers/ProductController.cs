using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Linq;
using Web.Mvc;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Web.Framework;
using BDMall.Enums;

namespace BDMall.Admin.Controllers
{

    /// <summary>
    /// 產品Controller
    /// </summary>
   // [ActionAuthorize(Module = ModuleConst.ProductModule)]
    public class ProductController : BaseMvcController
    {
        public ProductController(IComponentContext service) : base(service)
        {
        }

        /// <summary>
        /// 普通產品主頁
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductIndex()
        {
            var statusList = new List<int>() { };
            ViewBag.StatusList = JsonUtil.ToJson(statusList);

            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();

            ViewBag.CanEdit = 0;
            foreach (var role in CurrentUser.Roles)
            {
                if (role.PermissionList.Any(x => string.Equals(x.Function, FunctionConst.Prod_Edit.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    ViewBag.CanEdit = 1; break;
                }
            }
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        /// <summary>
        /// 待審批的產品主頁
        /// </summary>
        /// <returns></returns>
        public ActionResult WaitingApproveProduct()
        {
            //var statusList = new List<int>()
            //{
            //    (int)ProductStatus.WaitingApprove,
            //};
            //ViewBag.StatusList = JsonConvert.SerializeObject(statusList);

            //ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            ViewBag.Lang = CurrentUser.Lang;
            return View("ProductIndex");
        }

        /// <summary>
        /// 主頁
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CataLogIndex(string id)
        {
            ViewBag.Type = id ?? "1";
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        public ActionResult SelectCatalog()
        {
            ViewBag.Type = "2";
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }
        /// <summary>
        /// 目錄修改頁面
        /// </summary>
        /// <param name="id">节点Id</param>
        /// <param name="para2">父节点ID|父节点路径|父节点Level</param>
        /// <returns></returns>
        public ActionResult EditCatalog(string id, string para2)
        {
            ViewBag.LimitSize = 2048;//_settingBLL.GetCatalogImageLimtSize();
            ViewBag.Id = id;
            ViewBag.ParentInfo = para2 ?? "";

            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        /// <summary>
        /// 選擇產品頁面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="para2">商家ID</param>
        /// <param name="para3">搜索产品的类型</param>
        /// <returns></returns>
        public ActionResult SelectProduct(int id, string para2, string para3)
        {
            //ViewBag.IsSingleSelect = id;
            //ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            //ViewBag.MerchantId = para2 ?? Guid.Empty.ToString();
            //ViewBag.SearchProductType = para3;
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }



        /// <summary>
        /// 產品增刪改頁面
        /// </summary>
        /// <param name="id">产品ID</param>
        /// <param name="para2">新增、修改、复制、只讀</param>
        /// <returns></returns>
        public ActionResult EditProduct(string id, string para2)
        {
            ViewBag.Id = id;
            ViewBag.Type = para2;

            if (!HasPermission(FunctionConst.Prod_Edit))
            {
                //HasPermission(FunctionConst.Prod_View)
                ViewBag.EditType = "";
            }
            else
            {
                ViewBag.EditType = "Edit";
            }

            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();


            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        /// <summary>
        /// 上傳產品圖片頁面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="para2"></param>
        /// <returns></returns>
        public ActionResult ProductImg(Guid id, bool para2)
        {
            //ViewBag.LimitSize = 2048;// _settingBLL.GetProductImageLimtSize();
            //ViewBag.Id = id;
            //ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            //ViewBag.IsApprove = para2 ? 1 : 0;
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        /// <summary>
        /// 產品配件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AccessoriesProduct(int id)
        {
            ViewBag.Sku = id;
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        /// <summary>
        /// 關聯產品頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RelatedProduct(string id)
        {
            //ViewBag.Id = id;
            //ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        /// <summary>
        /// 組合產品詳細頁面
        /// </summary>
        /// <param name="id">sku</param>
        /// <param name="para2">是否為組合產品</param>
        /// <returns></returns>
        public ActionResult CombinationProduct(int id, int para2)
        {
            ViewBag.Sku = id;
            ViewBag.IsConfirm = para2;
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        /// <summary>
        /// 產品屬性主頁
        /// </summary>
        /// <returns></returns>
        public ActionResult ProdAttributeIndex()
        {
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        /// <summary>
        /// 屬性增刪改頁面
        /// </summary>
        /// <param name="id">屬性ID</param>
        /// <param name="para2">是否仓库属性</param>
        /// <returns></returns>
        public ActionResult EditAttribute(string id, string para2)
        {
            ViewBag.LimitSize = 2048;// _settingBLL.GetAttributeImageLimitSize();
            ViewBag.AttrID = id;
            ViewBag.IsInv = para2;
           
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();
            ViewBag.Lang = CurrentUser.Lang;

            return View();
        }

        /// <summary>
        /// 自定義價格範圍頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomPriceRange()
        {
            ViewBag.Lang = CurrentUser.Lang;

            return View();
        }

        /// <summary>
        /// 自定義價格範圍編輯頁面
        /// </summary>
        /// <param name="id">記錄ID</param>
        /// <param name="para2">編輯方式</param>
        /// <returns></returns>
        public ActionResult CustomPrcRngEdit(int id, string para2)
        {
            ViewBag.Id = id;
            ViewBag.EditType = para2;
            ViewBag.Lang = CurrentUser.Lang;

            return View();
        }

        /// <summary>
        /// 匯出產品Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportData()
        {
            ViewBag.Lang = CurrentUser.Lang;

            return View();
        }


        /// <summary>
        /// 批量更新catalog下所有產品價格頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdPriceIndex()
        {
            ViewBag.Lang = CurrentUser.Lang;

            return View();
        }

        /// <summary>
        /// 產品審批頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveProduct()
        {
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        /// <summary>
        /// 產品審批詳細頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ApproveProductDetail(string id)
        {
            ViewBag.Id = id;
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        /// <summary>
        /// 產品的審批歷史
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ApproveProductHistory(string id)
        {
            ViewBag.Id = id;
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        public ActionResult ProductClickRate()
        {
            //ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        public ActionResult ProductSearchRate()
        {
            //ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        public async Task<ActionResult> MassUploadProduct()
        {
            //ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            //string token = HttpContext.Request.Cookies[StoreConst.AccessToken]?.Value;

            //var codes = await HttpUtil.DoGet<List<CodeMaster>>(token, Runtime.Setting.PMServer, "/AdminApi/Setting/GetMassUploadSetting", null);

            //ViewBag.CSVMaxSize = int.Parse(codes.FirstOrDefault(p => p.Key == "CSVMaxSize")?.Value ?? "1024");
            //ViewBag.ImageMaxSize = int.Parse(codes.FirstOrDefault(p => p.Key == "ImageMaxSize")?.Value ?? "2048");
            return View();
        }


        public ActionResult MassUploadDetail(string id, string para2)
        {
            ViewBag.Id = id;
            ViewBag.IsFinished = para2;
            ViewBag.Lang = CurrentUser.Lang;

            return View();
        }

        public async Task<ActionResult> MassUploadInstoreProduct()
        {
            //ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            //string token = HttpContext.Request.Cookies[StoreConst.AccessToken]?.Value;

            //var codes = await HttpUtil.DoGet<List<CodeMaster>>(token, Runtime.Setting.PMServer, "/AdminApi/Setting/GetMassUploadSetting", null);

            //ViewBag.CSVMaxSize = int.Parse(codes.FirstOrDefault(p => p.Key == "CSVMaxSize")?.Value ?? "1024");
            //ViewBag.ImageMaxSize = int.Parse(codes.FirstOrDefault(p => p.Key == "ImageMaxSize")?.Value ?? "2048");
            return View();
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="condition"></param>
        ///// <returns></returns>
        //[HttpPost]
        ////public JsonResult ExportProduct(string code, string dateFrom, string dateTo, string lang)
        //public JsonResult ExportProduct(ProdSearchCond condition)
        //{
        //    string returnPath = "";
        //    //IProductBLL proBLL = BLLFactory.Create(CurrentWebStoreConfig).CreateProductBLL();


        //    //condition.PageInfo.PageSize = 0;
        //    //try
        //    //{
        //    //    var data = proBLL.Search(condition);
        //    //    List<ProductExcel> products = new List<ProductExcel>();
        //    //    foreach (var item in data.Data)
        //    //    {
        //    //        ProductExcel product = new ProductExcel();
        //    //        product.Sku = item.Sku;
        //    //        product.Code = item.Code;
        //    //        product.CategoryPath = item.CatalogPathName;

        //    //        product.Currency = item.Currency.Name;
        //    //        product.IsCombProcut = item.IsComb == true ? "Yes" : "No";
        //    //        product.ListPrice = (decimal)item.ListPrice;
        //    //        product.ProductNames = item.Name;
        //    //        product.SalePrice = (decimal)item.SalePrice;

        //    //        products.Add(product);
        //    //    }
        //    //    returnPath = CreateExcel(products);
        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //}

        //    return Json(returnPath, JsonRequestBehavior.AllowGet);

        //}


        //private string CreateExcel(List<ProductExcel> products)
        //{
        //    //byte[] bytes;
        //    using (ExcelPackage package = new ExcelPackage())
        //    {

        //        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Product");
        //        //worksheet.Cells.Style.ShrinkToFit = true;
        //        AddExcelHeader(worksheet, new ProductExcel());
        //        int line = 2;
        //        foreach (ProductExcel item in products)
        //        {
        //            Type type = item.GetType();
        //            PropertyInfo[] infos = type.GetProperties();//獲取有多少個欄位
        //            int i = 1;
        //            foreach (PropertyInfo info in infos)
        //            {
        //                object value = info.GetValue(item, null);
        //                if (value == null)
        //                {
        //                    worksheet.Cells[line, i].Value = "";
        //                }
        //                else
        //                {
        //                    worksheet.Cells[line, i].Value = value.ToString();
        //                }
        //                i++;
        //            }
        //            line++;
        //        }
        //        string returnPath = PathUtil.GetSiteRoot() + "/ClientResources/" + BDMall.Runtime.Setting.AppId + "/temp";
        //        string path = PathUtil.GetTempFileDirectory(BDMall.Runtime.Setting.ClientId);
        //        string fileName = CurrentUser.Email + ".xlsx";
        //        FileInfo file = new FileInfo(Path.Combine(path, fileName));
        //        package.SaveAs(file);
        //        //bytes = package.GetAsByteArray();
        //        return returnPath + "/" + fileName;
        //    }

        //}

        //private void AddExcelHeader(ExcelWorksheet worksheet, ProductExcel product)
        //{

        //    Type type = product.GetType();
        //    PropertyInfo[] infos = type.GetProperties();
        //    int i = 1;
        //    foreach (PropertyInfo item in infos)
        //    {
        //        worksheet.Cells[1, i].Value = item.Name;
        //        i++;
        //    }
        //}

        //private void OutputClient(byte[] bytes)
        //{
        //    //HttpContext.Response response;

        //    HttpContext.Response.Buffer = true;

        //    HttpContext.Response.Clear();
        //    HttpContext.Response.ClearHeaders();
        //    HttpContext.Response.ClearContent();

        //    HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    HttpContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")));

        //    HttpContext.Response.Charset = "utf-8";
        //    HttpContext.Response.ContentEncoding = Encoding.GetEncoding("utf-8");

        //    HttpContext.Response.BinaryWrite(bytes);
        //    HttpContext.Response.Flush();

        //    HttpContext.Response.Close();
        //}

        ///// <summary>
        ///// 產品時段價格
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ProdPeriodPriceIndex()
        //{
        //    ViewBag.MerchantId = CurrentUser.MerchantId;
        //    ViewBag.MerchantName = CurrentUser.MerchantName;

        //    ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;

        //    return View();
        //}

        /// <summary>
        /// 產品增刪改頁面
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="para2">新增、修改、复制、只讀</param>
        /// <returns></returns>
        public ActionResult EditProdPeriodPrice(string id, string para2)
        {
            //ViewBag.Id = id;
            //ViewBag.Type = para2;
            //ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            //ViewBag.MerchantId = CurrentUser.MerchantId;
            //if (!HasPermission(FunctionConst.Prod_Edit))
            //{
            //    //HasPermission(FunctionConst.Prod_View)
            //    ViewBag.EditType = "";
            //}
            //else
            //{
            //    ViewBag.EditType = "Edit";
            //}

            return View();
        }

    }
}