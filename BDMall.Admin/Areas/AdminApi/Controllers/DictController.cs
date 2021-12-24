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
    public class DictController : BaseApiController
    {
        ICodeMasterBLL codeMasterBLL;
        ISettingBLL settingBLL;
        public DictController(IComponentContext services) : base(services)
        {
            codeMasterBLL = Services.Resolve<ICodeMasterBLL>();
            settingBLL = Services.Resolve<ISettingBLL>();
        }

        /// <summary>
        /// 獲取系統支持的語言種類
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValue> GetSupportLanguage()
        {
            List<KeyValue> list = new List<KeyValue>();
            string userLang = CurrentUser.Lang.ToString();
            try
            {

                var langs = settingBLL.GetSupportLanguages();

                foreach (var item in langs)
                {
                    KeyValue entity = new KeyValue();
                    entity.Id = item.Code;
                    entity.Text = item.Text;
                    list.Add(entity);
                }
            }
            catch (BLException blex)
            {

            }

            return list;
        }

    }
}
