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
using Web.Framework;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
    [ApiController]
    public class MailServerController : BaseApiController
    {
        IMailServerBLL _mailServerBLL;

        public MailServerController(IComponentContext services) : base(services)
        {
            _mailServerBLL = Services.Resolve<IMailServerBLL>();
        }

        [HttpGet]
        public MailServerInfo GetById(Guid id)
        {
            return _mailServerBLL.GetById(id);
        }

        [HttpPost]
        public PageData<MailServerView> GetMailServerSettings([FromForm]MailServerCond cond)
        {
            return _mailServerBLL.GetMailServerSettings(cond);
        }



        [HttpPost]
        public SystemResult SaveMailServer([FromForm] MailServerInfo info)
        {
            return _mailServerBLL.SaveMailServer(info);
        }


        [HttpGet]
        public SystemResult DeleteMailServer(string ids)
        {
            var idArr = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return _mailServerBLL.DeleteMailServer(idArr);
        }

        [HttpGet]
        public SystemResult GetMailServerByMail(string mail)
        {
            return _mailServerBLL.GetMailServerByMail(mail);
        }
    }

}
