using Autofac;
using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class FileUploadController : BaseApiController
    {
        public FileUploadController(IComponentContext services) : base(services)
        {
        }

        [HttpPost]
        public async Task<SystemResult> UploadFile()
        {
            SystemResult result = new SystemResult();
            var oFiles = (FormFileCollection)this.CurrentContext.HttpContext.Request.Form.Files;
            CheckFileFormat(oFiles);
            string tempPath = PathUtil.GetPhysicalPath(Configuration["UploadPath"], Guid.Parse(CurrentUser.UserId), FileFolderEnum.TempPath);
            string relatePath = PathUtil.GetRelativePath(Guid.Parse(CurrentUser.UserId), FileFolderEnum.TempPath);

            if (!Directory.Exists(tempPath))          
                Directory.CreateDirectory(tempPath);
            
            var fileInfos = new List<UploadFileInfo>();
            for (int i = 0; i < oFiles.Count; i++)
            {
                var file = oFiles[i];
                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(tempPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);         //异步流写入到临时目录

                FileInfo of = new FileInfo(filePath);
                var uploadResult = new UploadFileInfo
                {
                    Path = filePath,
                    ThumbnailPath = "",
                    Thumbnail = "",
                    Name = fileName,
                    Size = of.Length,
                    Extension = of.Extension
                };
                uploadResult.OriginalName = file.FileName;
                uploadResult.Path = relatePath + "/" + fileName;
                uploadResult.Thumbnail = CreateThumbnail(fileName);
                uploadResult.ThumbnailPath = relatePath + "/" + uploadResult.Thumbnail;
                fileInfos.Add(uploadResult);
            }

            result.ReturnValue = fileInfos;
            result.Succeeded = true;
            return result;
        }

        [NonAction]
        private void CheckFileFormat(FormFileCollection files)
        {
            if (files.Count > 5)         
                throw new BLException(Resources.Message.FileNotMoreThan + " 5");
            
            if (files.Any(x=>!x.ContentType.Contains("image")))
                throw new BLException(Resources.Message.OnlyUploadImage);
        }

        [NonAction]
        private string CreateThumbnail(string originalImageName)
        {
            string thumbnaliName = string.Empty;

            string path = PathUtil.GetPhysicalPath(Configuration["UploadPath"], Guid.Parse(CurrentUser.UserId), FileFolderEnum.TempPath);
            thumbnaliName = Path.GetFileNameWithoutExtension(originalImageName) + "_s" + Path.GetExtension(originalImageName);

            ImageUtil.CreateImg(Path.Combine(path, originalImageName), path, thumbnaliName, 100, 100);//生成100*100的缩略图

            return thumbnaliName;
        }
    }
}
