
using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Utility;
using Intimex.Common;
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
    //[ServiceFilter(typeof(AdminApiAuthorizeAttribute))]
    public class EmailTypeItemsController : BaseApiController
    {
        IEmailTemplateBLL _emailTemplateBLL;
        ICodeMasterBLL _codeMasterBLL;

        public EmailTypeItemsController(IComponentContext service) : base(service)
        {
            _emailTemplateBLL = this.Services.Resolve<IEmailTemplateBLL>();
            _codeMasterBLL = this.Services.Resolve<ICodeMasterBLL>();
        }

        /// <summary>
        /// 顯示EmailType列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CodeMasterDto> Search()
        {
            List<CodeMasterDto> list = new List<CodeMasterDto>();

            //string lang = WSCookie.GetUserLanguage();

            list = _codeMasterBLL.GetCodeMasters(CodeMasterModule.System, CodeMasterFunction.EmailType);

            return list;
        }

        /// <summary>
        /// 獲取郵件類型信息
        /// </summary>
        /// <param name="emailTypeId"></param>
        /// <returns></returns>
        [HttpGet]

        public EmailTypeItemsView GetById(MailType emailType)
        {
            EmailTypeItemsView emailTypeItems = new EmailTypeItemsView();
            //string lang = WSCookie.GetUserLanguage();
            try
            {

                emailTypeItems = _emailTemplateBLL.GetEmailTypeItem(emailType);

            }
            catch (BLException blex)
            {

            }


            return emailTypeItems;
        }

        /// <summary>
        /// 根據郵件類型獲取對應的關鍵字
        /// </summary>
        /// <param name="emailTypeId">郵件類型</param>
        /// <returns></returns>
        [HttpGet]

        public List<EmailTempItemDto> GetEmailTypeItems(string emailTypeId)
        {
            List<EmailTempItemDto> list = new List<EmailTempItemDto>();
            try
            {
                if (!string.IsNullOrEmpty(emailTypeId))
                {
                    MailType type = (MailType)Enum.Parse(typeof(MailType), emailTypeId);
                    list = _emailTemplateBLL.GetEmailTempItem(type);
                }

            }
            catch (BLException blex)
            {

            }
            // bll = BLLFactory.Create(CurrentWebStore).CreateCodeMasterBLL();

            return list;
        }

        /// <summary>
        /// 保存郵件類型信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]

        public SystemResult Save([FromForm] EmailTypeItemsView entity)
        {
            SystemResult result;
            try
            {

                result = _emailTemplateBLL.SaveEmailTypeItems(entity);

            }
            catch (BLException blex)
            {
                result = new SystemResult();
                result.Succeeded = false;
                result.Message = blex.Message;
            }

            return result;
        }
    }
}
