
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
    public class EmailTempItemController : BaseApiController
    {
        IEmailTemplateBLL _emailTemplateBLL;

        public EmailTempItemController(IComponentContext service) : base(service)
        {
            _emailTemplateBLL = this.Services.Resolve<IEmailTemplateBLL>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [HttpPost]
        public List<EmailTempItemDto> Search([FromForm]EmailTempItemCondition cond)
        {
            //SystemResult result = new SystemResult();
            List<EmailTempItemDto> list = new List<EmailTempItemDto>();

            cond.IsDeleted = false;
            list = _emailTemplateBLL.FindTempItem(cond);

            return list;
        }
        /// <summary>
        /// 通過Id獲取郵件模板關鍵字
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public EmailTempItemDto GetById(Guid Id)
        {


            EmailTempItemDto emailTempItem = _emailTemplateBLL.GetTempItem(Id);
            return emailTempItem;


        }

        /// <summary>
        /// 保存郵件模板關鍵字
        /// </summary>
        /// <param name="tempItem"></param>
        /// <returns></returns>
        [HttpPost]
        public SystemResult Save([FromForm] EmailTempItemDto tempItem)
        {
            SystemResult result = new SystemResult();
            try
            {

                if (tempItem.Id == Guid.Empty)
                {
                    result = _emailTemplateBLL.AddTempItem(tempItem);
                }
                else
                {
                    result = _emailTemplateBLL.UpdateTempItem(tempItem);
                }
            }
            catch (BLException blex)
            {
                result.Succeeded = false;
            }
            return result;

        }

        /// <summary>
        /// 刪除郵件模板關鍵字
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public SystemResult DeleteItem(string ids)
        {
            SystemResult result = null;

            try
            {
                string[] datas = ids.Split(',');
                if (datas.Length > 0)
                {
                    var guids = datas.Select(d => new Guid(d)).ToList();
                    result = _emailTemplateBLL.DeleteTempItem(guids);
                }
                else
                {
                    result = new SystemResult();
                    result.Message = "參數不正確";
                }


            }
            catch (BLException blex)
            {
                result = new SystemResult();
                result.Succeeded = false;
                result.Message = blex.Message;
            }
            return result;
        }

        /// <summary>
        /// 獲取EmailItems的下拉框內容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValue> GetEmailItems()
        {
            List<KeyValue> list = new List<KeyValue>();

            var userLang = CurrentUser.Lang;



            EmailTempItemCondition cond = new EmailTempItemCondition();
            cond.Value = "";

            var emailItems = _emailTemplateBLL.FindTempItem(cond);

            list = EmailItemsToKeyValue(emailItems);
            return list;

        }

        private List<KeyValue> EmailItemsToKeyValue(List<EmailTempItemDto> items)
        {
            List<KeyValue> list = new List<KeyValue>();
            foreach (var item in items)
            {
                KeyValue keyValue = new KeyValue();
                keyValue.Id = item.Id.ToString();
                keyValue.Text = item.Description;
                list.Add(keyValue);
            }
            return list;
        }
    }
}
