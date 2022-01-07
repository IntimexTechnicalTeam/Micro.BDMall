
using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Utility;
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
    public class EmailTemplateController : BaseApiController
    {
        IEmailTemplateBLL _emailTemplateBLL;

        public EmailTemplateController(IComponentContext service) : base(service)
        {
            _emailTemplateBLL = this.Services.Resolve<IEmailTemplateBLL>();
        }

        //[HttpPost]
        //public SystemResult Save(EmailTemplateDto template)
        //{
        //    SystemResult result = new SystemResult();

        //    if (template.Id == new Guid())
        //    {
        //        result = _emailTemplateBLL.AddTemplate(template);
        //    }
        //    else
        //    {
        //        result = _emailTemplateBLL.UpdateTemplate(template);
        //    }
        //    return result;

        //}

        ///// <summary>
        ///// 獲取郵件模板
        ///// </summary>
        ///// <param name="cond"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public List<EmailTemplateDto> Search(EmailTempCondition cond)
        //{
        //    //SystemResult result = new SystemResult();
        //    List<EmailTemplateDto> list = new List<EmailTemplateDto>();
        //    list = _emailTemplateBLL.FindTemplate(cond);
        //    return list;
        //}

        ///// <summary>
        ///// 根據模板Id獲取模板資料
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public EmailTemplateDto GetById(string Id)
        //{
        //    EmailTemplateDto EmailTemplateDto = new EmailTemplateDto();
        //    EmailTemplateDto = _emailTemplateBLL.GetTemplate(new Guid(Id));
        //    return EmailTemplateDto;
        //}


        ///// <summary>
        ///// 將選中的EmailTemplateDto設置為已刪除
        ///// </summary>
        ///// <param name="tids"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public SystemResult Delete(string tids)
        //{
        //    SystemResult result = new SystemResult();
        //    var arrTids = tids.Split(',');
        //    foreach (string item in arrTids)
        //    {
        //        _emailTemplateBLL.DeleteTemplate(new Guid(item));
        //    }
        //    result.Succeeded = true;
        //    return result;
        //}

        ///// <summary>
        ///// 设置选中的EmailTemplateDto为有效，与该EmailTemplateDto相同EmailType的其它template设置为失效
        ///// </summary>
        ///// <param name="tid"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public SystemResult SetActive(Guid tid)
        //{
        //    SystemResult result = new SystemResult();
        //    _emailTemplateBLL.EnableTemplate(tid);

        //    return result;
        //}
    }
}
