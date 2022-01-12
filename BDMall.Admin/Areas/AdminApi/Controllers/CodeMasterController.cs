using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using Intimex.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Intimex.Common;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using BDMall.Domain;
using BDMall.Enums;
using System.Threading;
using System.Globalization;
using BDMall.BLL;

namespace BDMall.Admin.Areas.AdminAPI.Controllers
{

    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
    public class CodeMasterController : BaseApiController
    {
        ICodeMasterBLL CodeMasterBLL;

        public CodeMasterController(IComponentContext services) : base(services)
        {
            CodeMasterBLL = Services.Resolve<ICodeMasterBLL>(); 
        }

        [HttpPost]
        public PageData<CodeMasterDto> GetCodeMasters(CodeMasterCondition con)
        {
            PageData<CodeMasterDto> list = new PageData<CodeMasterDto>();

            list = CodeMasterBLL.GetCodeMastersByPage(con);//.GetCodeMasters(con.Module, con.Function, con.Value);


            return list;
        }

        /// <summary>
        /// 獲取字碼主檔列表
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        [HttpPost]
        public PageData<CodeMasterDto> GetCodeMastersByPage(CodeMasterCondition con)
        {
            PageData<CodeMasterDto> resukt = new PageData<CodeMasterDto>();

            resukt = CodeMasterBLL.GetCodeMastersByPage(con);


            return resukt;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetById")]
        public CodeMasterDto GetById(int id)
        {
            CodeMasterDto entity = new CodeMasterDto();


            entity = CodeMasterBLL.GetCodeMasterById(id);


            return entity;
        }

        /// <summary>
        /// 根據傳入的三個key獲取codeMaster信息
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [HttpPost]
        public CodeMasterDto GetCodeMasterByKey([FromForm]CodeMasterCondition cond)
        {


            return CodeMasterBLL.GetCodeMasterByKey( cond.Module, cond.Function, cond.Value);


        }

        /// <summary>
        /// 保存字碼主檔
        /// </summary>
        /// <param name="codeMaster"></param>
        /// <returns></returns>
        [HttpPost]
        public SystemResult Save([FromForm]CodeMasterDto codeMaster)
        {
            SystemResult result = new SystemResult();


            if (codeMaster.Id == 0)
            {
                CodeMasterBLL.InsertCodeMaster(codeMaster);
            }
            else
            {
                CodeMasterBLL.UpdateCodeMaster(codeMaster);
            }
            result.Succeeded = true;

            return result;
        }

        /// <summary>
        /// 根據字碼主檔記錄Id進行邏輯刪除
        /// </summary>
        /// <param name="cIds"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Delete")]
        public SystemResult Delete(string cIds)
        {
            SystemResult result = new SystemResult();
            var arrId = cIds.Split(',');

            foreach (var cId in arrId)
            {
                CodeMasterBLL.DeleteCodeMaster(int.Parse(cId));
            }
            result.Succeeded = true;

            return result;
        }
        [HttpGet]
        [ActionName("ActDelete")]
        public SystemResult ActDelete(string id)
        {
            SystemResult result = new SystemResult();



            result.Succeeded = CodeMasterBLL.ActualDelete(int.Parse(id));


            return result;
        }

        /// <summary>
        /// 获取平台基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<CodeMasterDto> GetMallInfo()
        {
            return CodeMasterBLL.GetMallInfo();
        }
        /// <summary>
        /// 获取平台基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public SystemResult UpdateMallInfo([FromForm] MallInfoView info)
        {
            SystemResult result = new SystemResult();

            CodeMasterBLL.UpdateMallInfo(info.CodeMaster);
            result.Succeeded = true;
            return result;
        }

    }
}
