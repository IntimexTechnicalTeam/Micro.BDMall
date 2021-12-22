using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    public class ProdCatalogController : BaseApiController
    {
        IProductCatalogBLL productCatalogBLL;
        
        public ProdCatalogController(IComponentContext services) : base(services)
        {
            productCatalogBLL = Services.Resolve<IProductCatalogBLL>();          
        }

        [HttpGet]
        public async Task<SystemResult> GetAllCatalog()
        {
            SystemResult result = new SystemResult();
            result.Succeeded = true;
            result.ReturnValue = productCatalogBLL.GetAllCatalog();
            return result;
        }

        [HttpGet]
        public SystemResult GetCatalogTree()
        {
            SystemResult result = new SystemResult();
            List<ProductCatalogEditModel> list = new List<ProductCatalogEditModel>();
            list = productCatalogBLL.GetCatalogTree(false);
            result.Succeeded = true;
            result.ReturnValue = list;
            return result;
        }

        public SystemResult GetActiveCatalogTree()
        {
            SystemResult result = new SystemResult();
            List<ProductCatalogEditModel> list = new List<ProductCatalogEditModel>();
            list = productCatalogBLL.GetCatalogTree(true);
            result.Succeeded = true;
            result.ReturnValue = list;
            return result;
        }

        [HttpGet]
        public SystemResult GetById(string id)
        {
            SystemResult result = new SystemResult();
            ProductCatalogEditModel catalog = new ProductCatalogEditModel();
            catalog = productCatalogBLL.GetCatalog(Guid.Parse(id));

            result.Succeeded = true;
            result.ReturnValue = catalog;
            return result;
        }

        [HttpGet]
        public async Task<SystemResult> Delete(Guid id)
        {
            SystemResult result = new SystemResult();
            productCatalogBLL.DeleteCatalog(id);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 更新各個catalog的順序
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SystemResult> UpdateSeq([FromBody]List<ProductCatalogEditModel> tree)
        {
            SystemResult result = new SystemResult();
            var list = TreeToList(tree);
            list = list.Where(p => p.IsChange == true).ToList();
            await productCatalogBLL.UpdateCatalogSeqAsync(list);
            result.Succeeded = true;
            return result;
        }

        [HttpGet]
        public async Task<SystemResult> DisActiveCatalog(Guid catId)
        {
            SystemResult result = new SystemResult();
            result = await productCatalogBLL.DisActiveCatalogAsync(catId);
            return result;
        }

        [HttpGet]
        public async Task<SystemResult> ActiveCatalog(Guid catId)
        {
            SystemResult result = new SystemResult();
            result = await productCatalogBLL.ActiveCatalogAsync(catId);
            return result;
        }

        [HttpPost]
        public async Task<SystemResult> Save([FromForm] ProductCatalogEditModel catalog)
        {
            SystemResult result = new SystemResult();

            catalog.Validate();

            string tempName = catalog.SmallIcon;
            string tempNameM = catalog.MSmallIcon;
            if (catalog.Id == Guid.Empty)
            {
                catalog.Id = Guid.NewGuid();
                catalog.Action = ActionTypeEnum.Add;
            }
            else
            {
                catalog.Action = ActionTypeEnum.Modify;
            }

            GenImagePath(catalog);
            result = await productCatalogBLL.SaveCatalog(catalog);
        
            CreateImage(catalog.Id, tempName, null);
            CreateImage(catalog.Id, tempNameM, "_m");

            //string key = CacheKey.MenuCatalog.ToString();
            //var fields = GetSupportLanguage().Select(s => $"{CacheField.SubCatalog.ToString()}_{s.Code}").ToArray();
            //RedisHelper.HDel(key, fields);

            //key = CacheKey.PreferenceCatalog.ToString();
            //fields = SettingBLL.GetSupportLanguages().Select(s => $"{CacheField.defaultCatalog.ToString()}_{s.Code}").ToArray();
            //RedisHelper.HDel(key, fields);

            result.Succeeded = true;
            return result;
        }

        [NonAction]
        private List<ProductCatalogEditModel> TreeToList(List<ProductCatalogEditModel> tree)
        {
            List<ProductCatalogEditModel> list = new List<ProductCatalogEditModel>();

            list = GenList(tree);

            return list;
        }

        [NonAction]
        private List<ProductCatalogEditModel> GenList(List<ProductCatalogEditModel> tree)
        {
            List<ProductCatalogEditModel> list = new List<ProductCatalogEditModel>();
            foreach (ProductCatalogEditModel item in tree)
            {
                //if (item.IsChange == true)
                //{
                list.Add(item);
                if (item.Children != null)
                {
                    list.AddRange(GenList(item.Children));
                }
                //}
            }

            return list;

        }

        [NonAction]
        private List<string> CreateImage(Guid catId, string tempFileName, string nameSign)
        {
            List<string> data = new List<string>();

            string tempFolder = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], Guid.Parse(CurrentUser.UserId), FileFolderEnum.TempPath);
            
            if (!string.IsNullOrEmpty(tempFileName))
            {
                string imageName = Path.GetFileName(tempFileName);
                string tempFilePath = Path.Combine(tempFolder, imageName);
                var file = new FileInfo(tempFilePath);
                if (file.Exists)
                {
                    string fileExtension = Path.GetExtension(tempFileName);
                    string originalImg = catId + nameSign + "_o" + fileExtension;
                    string smallImg = catId + nameSign + "_s" + fileExtension;
                    string bigImg = catId + nameSign + "_b" + fileExtension;

                    string relativePath = PathUtil.GetRelativePath(Guid.Parse(CurrentUser.UserId), FileFolderEnum.Catalog);
                    string localPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], Guid.Parse(CurrentUser.UserId), FileFolderEnum.Catalog);

                    //var smallImageSize = _settingBLL.GetSmallCatalogImageSize();
                    //var bigImageSize = _settingBLL.GetBigCatalogImageSize();

                    ImageUtil.CreateImg(tempFilePath, localPath, smallImg, 100, 100);//生成catalog的小图
                    ImageUtil.CreateImg(tempFilePath, localPath, bigImg, 400, 400);//生成catalog的大图
                                                                                   //将原图从Temp文件夹移动到newPath
                    FileUtil.MoveFile(tempFilePath, localPath, originalImg);

                    var smallIcon = relativePath + "/" + smallImg;
                    var bigIcon = relativePath + "/" + bigImg;
                    var originalIcon = relativePath + "/" + originalImg;

                    smallIcon = smallIcon.Replace("/", "\\");
                    bigIcon = bigIcon.Replace("/", "\\");
                    originalIcon = originalIcon.Replace("/", "\\");

                    data.Add(smallIcon);
                    data.Add(bigIcon);
                    data.Add(originalIcon);
                }

            }
            return data;
        }

        [NonAction]
        private void GenImagePath(ProductCatalogEditModel catalog)
        {
            var tempFileName = catalog.SmallIcon;
            string tempFolder = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], Guid.Parse(CurrentUser.UserId), FileFolderEnum.TempPath);
            
            if (!string.IsNullOrEmpty(tempFileName))
            {
                string imageName = Path.GetFileName(tempFileName);
                string tempFilePath = Path.Combine(tempFolder, imageName);
                var file = new FileInfo(tempFilePath);
                if (file.Exists)
                {
                    string fileExtension = Path.GetExtension(tempFileName);
                    string originalImg = catalog.Id + "_o" + fileExtension;
                    string smallImg = catalog.Id + "_s" + fileExtension;
                    string bigImg = catalog.Id + "_b" + fileExtension;

                    string relativePath = PathUtil.GetRelativePath(Guid.Parse(CurrentUser.UserId), FileFolderEnum.Catalog);

                    var smallIcon = relativePath + "/" + smallImg;
                    var bigIcon = relativePath + "/" + bigImg;
                    var originalIcon = relativePath + "/" + originalImg;

                    smallIcon = smallIcon.Replace("/", "\\");
                    bigIcon = bigIcon.Replace("/", "\\");
                    originalIcon = originalIcon.Replace("/", "\\");

                    catalog.SmallIcon = smallIcon;
                    catalog.BigIcon = bigIcon;
                    catalog.OriginalIcon = originalIcon;
                }
            }

            var tempFileNameM = catalog.MSmallIcon;           
            if (!string.IsNullOrEmpty(tempFileNameM))
            {
                string imageNameM = Path.GetFileName(tempFileNameM);
                string tempFilePathM = Path.Combine(tempFolder, imageNameM);
                var file = new FileInfo(tempFilePathM);
                if (file.Exists)
                {
                    string fileExtension = Path.GetExtension(tempFileNameM);
                    string originalImg = catalog.Id + "_m_o" + fileExtension;
                    string smallImg = catalog.Id + "_m_s" + fileExtension;
                    string bigImg = catalog.Id + "_m_b" + fileExtension;

                    string relativePath = PathUtil.GetRelativePath(Guid.Parse(CurrentUser.UserId), FileFolderEnum.Catalog);

                    var smallIcon = relativePath + "/" + smallImg;
                    var bigIcon = relativePath + "/" + bigImg;
                    var originalIcon = relativePath + "/" + originalImg;

                    smallIcon = smallIcon.Replace("/", "\\");
                    bigIcon = bigIcon.Replace("/", "\\");
                    originalIcon = originalIcon.Replace("/", "\\");

                    catalog.MSmallIcon = smallIcon;
                    catalog.MBigIcon = bigIcon;
                    catalog.MOriginalIcon = originalIcon;
                }
            }
        }
    }
}
