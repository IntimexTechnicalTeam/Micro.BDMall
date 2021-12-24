using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using Intimex.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    public class SysCustomizationController : BaseApiController
    {
        ICodeMasterBLL codeMasterBLL;
        ISettingBLL settingBLL;
        public SysCustomizationController(IComponentContext services) : base(services)
        {
            codeMasterBLL = Services.Resolve<ICodeMasterBLL>();
        }

        /// <summary>
        /// 獲取系統定制化功能清單
        /// </summary>
        [HttpGet]
        public SysCustomization GetSysCustomization()
        {
            SysCustomization sysCustomization = new SysCustomization();
            sysCustomization = codeMasterBLL.GetSysCustomization();
            return sysCustomization;
        }

        /// <summary>
        /// 更新系統定制化功能
        /// </summary>
        /// <param name="funcCustom">系統定制功能清單</param>
        [HttpPost]
        public async Task<SystemResult> UpdateSysCustomization([FromForm] SysCustomization funcCustom)
        {
            SystemResult sysRslt = new SystemResult();
            sysRslt = codeMasterBLL.UpdateSysCustomization(funcCustom);
            return sysRslt;
        }
        [HttpPost]
        public async Task<SystemResult> SaveSystemLogo([FromForm] SystemLogo logo)
        {



            List<Action> actionList = new List<Action>();
            List<string> files = new List<string>();
            string tempPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], CurrentUser.UserId, Enums.FileFolderEnum.TempPath);
            string destPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], CurrentUser.UserId, Enums.FileFolderEnum.StoreLogo);
            if (!string.IsNullOrEmpty(logo.StoreLogo.ImageName))
            {
                string temp1 = Path.Combine(tempPath, logo.StoreLogo.ImageName);
                var file = new FileInfo(temp1);
                if (file.Exists)
                {
                    string extension = Path.GetExtension(logo.StoreLogo.ImageName);
                    string fileName = "storelogo" + extension;
                    logo.StoreLogo.ImageName = PathUtil.GetRelativePath(CurrentUser.UserId, Enums.FileFolderEnum.StoreLogo) + fileName;
                    Action a1 = new Action(() => { FileUtil.MoveFile(temp1, destPath, fileName); });
                    actionList.Add(a1);
                }
                string temp2 = Path.Combine(tempPath, logo.EmailLogo.ImageName);
                var file2 = new FileInfo(temp2);
                if (file2.Exists)
                {
                    string extension = Path.GetExtension(logo.EmailLogo.ImageName);
                    string fileName = "emaillogo" + extension;
                    logo.EmailLogo.ImageName = PathUtil.GetRelativePath(CurrentUser.UserId, Enums.FileFolderEnum.StoreLogo) + fileName;
                    Action a2 = new Action(() => { FileUtil.MoveFile(temp2, destPath, fileName); });
                    actionList.Add(a2);
                }
                string temp3 = Path.Combine(tempPath, logo.ReportLogo.ImageName);
                var file3 = new FileInfo(temp3);
                if (file3.Exists)
                {
                    string extension = Path.GetExtension(logo.ReportLogo.ImageName);
                    string fileName = "reportlogo" + extension;
                    logo.ReportLogo.ImageName = PathUtil.GetRelativePath(CurrentUser.UserId, Enums.FileFolderEnum.StoreLogo) + fileName;
                    Action a3 = new Action(() => { FileUtil.MoveFile(temp3, destPath, fileName); });
                    actionList.Add(a3);
                }
            }
            SystemResult sysRslt = new SystemResult();
            sysRslt = codeMasterBLL.SaveSystemLogo(logo);

            if (sysRslt.Succeeded)
            {
                try
                {
                    foreach (var item in actionList)
                    {
                        item();
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                }
            }
            return sysRslt;
        }

        [HttpGet]
        public async Task<SystemLogo> GetSystemLogos()
        {
            //SystemLogo sysRslt = new SystemLogo();
            //try
            //{
            //    sysRslt = CodeMasterBLL.GetSystemLogos();
            //}
            //catch (Exception ex)
            //{

            //}
            //return sysRslt;


            var data = codeMasterBLL.GetSystemLogos();
            data.StoreLogo.ImagePath += "?t=" + DateTime.Now.Ticks.ToString();
            data.ReportLogo.ImagePath += "?t=" + DateTime.Now.Ticks.ToString();
            data.EmailLogo.ImagePath += "?t=" + DateTime.Now.Ticks.ToString();

            return data;
        }


        [HttpGet]

        public async Task<SystemResult> GetProductImageLimtSize()
        {
            SystemResult result = new SystemResult();
            result.ReturnValue = settingBLL.GetProductImageLimtSize();
            result.Succeeded = true;
            return result;
        }

    }
}
