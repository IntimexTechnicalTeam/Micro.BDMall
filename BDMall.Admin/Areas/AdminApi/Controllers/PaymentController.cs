
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
    public class PaymentController : BaseApiController
    {
        IPaymentBLL _paymentBLL;

        public PaymentController(IComponentContext service) : base(service)
        {
            _paymentBLL = this.Services.Resolve<IPaymentBLL>();
        }

        /// <summary>
        /// 獲取支付方式列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<PaymentMethodDto> GetPaymentMenthodList()
        {
            //SystemResult result = new SystemResult();
            List<PaymentMethodDto> list = null;

            list = _paymentBLL.GetPaymentMenthods();

            return list;
        }

        /// <summary>
        /// 獲取付款賬戶
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PayAccount GetPayAccount()
        {

            PayAccount account = new PayAccount();

            string systemLang = CurrentUser.Lang.ToString();
            // IPaymentBLL bll = BLLFactory.Create(CurrentWebStore).CreatePaymentBLL();
            //account = bll.GetPayMentAccount();

            return account;
        }

        /// <summary>
        /// 獲取支付方式的詳細信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public PaymentMethodDto GetPayManagement(Guid id)
        {

            PaymentMethodDto paydetaidl = _paymentBLL.GetPaymentMenthod(id);
            return paydetaidl;
        }


        /// <summary>
        /// 保存支付方式信息
        /// </summary>
        /// <param name="payItems"></param>
        /// <returns></returns>
        [HttpPost]
        public SystemResult SavePayMethodsItem([FromForm]PaymentMethodDto payItems)
        {
            SystemResult result = new SystemResult();


            _paymentBLL.SavePayMethodItem(payItems);
            result.Succeeded = true;

            return result;

        }

        /// <summary>
        /// 保存支付账户信息
        /// </summary>
        /// <param name="payAccount"></param>
        /// <returns></returns>
        [HttpPost]
        public SystemResult SavePayAccount([FromForm] PayAccount payAccount)
        {
            SystemResult result = new SystemResult();



            // PaymentBLL.SavePayAccount(payAccount);
            result.Succeeded = true;
            result.Message = "";

            return result;

        }

        /// <summary>
        /// 刪除PayMethods信息
        /// </summary>
        /// <param name="tids">tids</param>
        /// <returns></returns>
        [HttpGet]
        public SystemResult DeletePayMethods(string tids)
        {

            SystemResult result = new SystemResult();

            Guid[] ids;
            string[] stringids = tids.Split(',');
            ids = Array.ConvertAll<string, Guid>(stringids, s => new Guid(s));

            _paymentBLL.DeletePayMethods(ids);

            result.Succeeded = true;

            return result;
        }

        /// <summary>
        /// 刪除PayMethods信息
        /// </summary>
        /// <param name="name">tids</param>
        /// <returns></returns>
        [HttpGet]
        public SystemResult DeletePicture(string name)
        {

            SystemResult result = new SystemResult();
            try
            {

                //IPaymentBLL bll = BLLFactory.Create(CurrentWebStore).CreatePaymentBLL();
                ////   bll.DeletePicture(name);

                //result.Succeeded = true;
            }
            catch (BLException blex)
            {

            }
            catch (Exception ex)
            {
                result.Succeeded = false;
            }

            return result;
        }
    }
}
