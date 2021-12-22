using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

            //return await ApiGet<SysCustomization>("/SysCustomization/GetSysCustomization", null, null);
        }

        /// <summary>
        /// 更新系統定制化功能
        /// </summary>
        /// <param name="funcCustom">系統定制功能清單</param>
        [HttpPost]
        public async Task<SystemResult> UpdateSysCustomization(SysCustomization funcCustom)
        {
            //SystemResult sysRslt = new SystemResult();
            //try
            //{
            //    sysRslt = CodeMasterBLL.UpdateSysCustomization(funcCustom);
            //}
            //catch (Exception ex)
            //{
            //    sysRslt.Message = ex.Message;
            //    sysRslt.Succeeded = false;
            //}
            //return sysRslt;

            return await ApiPost<SysCustomization, SystemResult>("/SysCustomization/UpdateSysCustomization", funcCustom);
        }
        [HttpPost]
        public async Task<SystemResult> SaveSystemLogo(SystemLogo logo)
        {
            //SystemResult sysRslt = new SystemResult();
            //try
            //{
            //    CodeMasterBLL.SaveSystemLogo(logo);
            //    sysRslt.Succeeded = true;
            //}
            //catch (Exception ex)
            //{
            //    sysRslt.Succeeded = false;
            //    sysRslt.Message = Resources.Message.SaveFail;
            //}
            //return sysRslt;


            List<Action> actionList = new List<Action>();
            List<string> files = new List<string>();
            string tempPath = PathUtil.GetTempFileDirectory(CurrentUser.ClientId);
            string destPath = PathUtil.GetStoreLogoImageFilePath(CurrentUser.ClientId);
            if (!string.IsNullOrEmpty(logo.StoreLogo.ImageName))
            {
                string temp1 = Path.Combine(tempPath, logo.StoreLogo.ImageName);
                if (File.Exists(temp1))
                {
                    string extension = Path.GetExtension(logo.StoreLogo.ImageName);
                    string fileName = "storelogo" + extension;
                    logo.StoreLogo.ImageName = PathUtil.GetStoreLogoImagePath(CurrentUser.ComeFrom, CurrentUser.ClientId, "/ClientResources/" + CurrentUser.ClientId + "/logo/" + fileName);
                    files.Add(logo.StoreLogo.ImageName);
                    Action a1 = new Action(() => { Intimex.Utility.FileUtil.MoveFile(temp1, destPath, fileName); });
                    actionList.Add(a1);
                }
                string temp2 = Path.Combine(tempPath, logo.EmailLogo.ImageName);
                if (File.Exists(temp2))
                {
                    string extension = Path.GetExtension(logo.EmailLogo.ImageName);
                    string fileName = "emaillogo" + extension;
                    logo.EmailLogo.ImageName = PathUtil.GetStoreLogoImagePath(CurrentUser.ComeFrom, CurrentUser.ClientId, "/ClientResources/" + CurrentUser.ClientId + "/logo/" + fileName);
                    files.Add(logo.EmailLogo.ImageName);
                    Action a2 = new Action(() => { Intimex.Utility.FileUtil.MoveFile(temp2, destPath, fileName); });
                    actionList.Add(a2);
                }
                string temp3 = Path.Combine(tempPath, logo.ReportLogo.ImageName);
                if (File.Exists(temp3))
                {
                    string extension = Path.GetExtension(logo.ReportLogo.ImageName);
                    string fileName = "reportlogo" + extension;
                    logo.ReportLogo.ImageName = PathUtil.GetStoreLogoImagePath(CurrentUser.ComeFrom, CurrentUser.ClientId, "/ClientResources/" + CurrentUser.ClientId + "/logo/" + fileName);
                    files.Add(logo.ReportLogo.ImageName);
                    Action a3 = new Action(() => { Intimex.Utility.FileUtil.MoveFile(temp3, destPath, fileName); });
                    actionList.Add(a3);
                }
            }


            SystemResult result = await ApiPost<SystemLogo, SystemResult>("/SysCustomization/SaveSystemLogo", logo);
            if (result.Succeeded)
            {
                try
                {
                    foreach (var item in actionList)
                    {
                        item.BeginInvoke(null, null);
                    }

                    ResourceDistribute.Distribute(files);
                }
                catch (Exception ex)
                {
                    _loger.Error(ex);
                    //throw;
                }
            }
            return result;
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


            var data = await ApiGet<SystemLogo>("/SysCustomization/GetSystemLogos", null, null);
            data.StoreLogo.ImagePath += "?t=" + DateTime.Now.Ticks.ToString();
            data.ReportLogo.ImagePath += "?t=" + DateTime.Now.Ticks.ToString();
            data.EmailLogo.ImagePath += "?t=" + DateTime.Now.Ticks.ToString();

            return data;
        }


        [HttpGet]

        public async Task<SystemResult> GetProductImageLimtSize()
        {
            return await ApiGet<SystemResult>("/SysCustomization/GetProductImageLimtSize", null, null);
        }

    }
}
