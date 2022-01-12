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
using BDMall.Utility;

namespace BDMall.Admin.Areas.AdminAPI.Controllers
{

    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
    public class PaymentGatewayController : BaseApiController
    {
        IPaymentGatewayBLL _paymentGatewayBLL;

        public PaymentGatewayController(IComponentContext services) : base(services)
        {
            _paymentGatewayBLL = Services.Resolve<IPaymentGatewayBLL>();
        }
        /// <summary>
        /// 根據類型獲取支付方式的客製化信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public SystemResult GetConfig(string type)
        {
            SystemResult result = new SystemResult();


            var pg = (PaymentGateType)Enum.Parse(typeof(PaymentGateType), type.ToUpper());
            var cm = _paymentGatewayBLL.GetConfig(pg);
            result.ReturnValue = cm;
            result.Succeeded = true;


            return result;
        }

        [HttpPost]
        public SystemResult SaveUnionConfig([FromForm] MPGSPayConfig data)
        {
            SystemResult result = new SystemResult();

            data.Gateway = PaymentGateType.MPGS2.ToString();
            data.ProxyHost = string.IsNullOrEmpty(data.ProxyHost) ? "" : data.ProxyHost;
            data.ProxyUser = string.IsNullOrEmpty(data.ProxyUser) ? "" : data.ProxyUser;
            data.ProxyPassword = string.IsNullOrEmpty(data.ProxyPassword) ? "" : data.ProxyPassword;
            data.ProxyDomain = string.IsNullOrEmpty(data.ProxyDomain) ? "" : data.ProxyDomain;
            data.GatewayHost = string.IsNullOrEmpty(data.GatewayHost) ? "ap-gateway.mastercard.com" : data.GatewayHost;
            data.MerchantName = string.IsNullOrEmpty(data.MerchantName) ? "" : data.MerchantName;
            result.Succeeded = _paymentGatewayBLL.SaveOrUpdateConfig(data);
            result.Message = Resources.Message.SaveSuccess;
            return result;
        }

        [HttpPost]
        public SystemResult SaveMasterConfig([FromForm] MPGSPayConfig data)
        {
            SystemResult result = new SystemResult();
            try
            {


                data.Gateway = PaymentGateType.MPGS1.ToString();
                data.ProxyHost = string.IsNullOrEmpty(data.ProxyHost) ? "" : data.ProxyHost;
                data.ProxyUser = string.IsNullOrEmpty(data.ProxyUser) ? "" : data.ProxyUser;
                data.ProxyPassword = string.IsNullOrEmpty(data.ProxyPassword) ? "" : data.ProxyPassword;
                data.ProxyDomain = string.IsNullOrEmpty(data.ProxyDomain) ? "" : data.ProxyDomain;
                data.GatewayHost = string.IsNullOrEmpty(data.GatewayHost) ? "ap-gateway.mastercard.com" : data.GatewayHost;
                data.MerchantName = string.IsNullOrEmpty(data.MerchantName) ? "" : data.MerchantName;
                data.UseSSL = true;
                data.IgnoreSslErrors = false;
                data.IsPassOnLocal = true;
                result.Succeeded = _paymentGatewayBLL.SaveOrUpdateConfig(data);
                result.Message = Resources.Message.SaveSuccess;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;

            }
            return result;
        }

        [HttpPost]
        public SystemResult SaveStripeConfig([FromForm] StripePayConfig data)
        {
            SystemResult result = new SystemResult();
            try
            {
                data.Gateway = PaymentGateType.STRIPE.ToString();
                result.Succeeded = _paymentGatewayBLL.SaveOrUpdateConfig(data);
                result.Message = Resources.Message.SaveSuccess;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;

            }
            return result;
        }

        [HttpPost]
        public SystemResult SavePayMeConfig([FromForm] PaymeConfig data)
        {
            SystemResult result = new SystemResult();
            try
            {

                data.Gateway = PaymentGateType.PAYME.ToString();
                data.Url = "https://api.payme.hsbc.com.hk";
                data.Ver = "0.12";
                result.Succeeded = _paymentGatewayBLL.SaveOrUpdateConfig(data);
                result.Message = Resources.Message.SaveSuccess;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;

            }
            return result;
        }

        [HttpPost]
        public SystemResult SavePaypalConfig([FromForm] PaypalConfig data)
        {
            SystemResult result = new SystemResult();
            try
            {

                data.Gateway = PaymentGateType.PAYPAL.ToString();
                result.Succeeded = _paymentGatewayBLL.SaveOrUpdateConfig(data);
                result.Message = Resources.Message.SaveSuccess;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;

            }
            return result;
        }


        [HttpPost]
        public SystemResult SaveWechatConfig([FromForm] WeChatPayConfig data)
        {
            SystemResult result = new SystemResult();
            try
            {

                data.Gateway = PaymentGateType.WECHAT.ToString();
                data.Version = "2.0";
                data.Service = "pay.weixin.native.intl";
                //data.Service = "pay.weixin.native";
                data.QueryService = "unified.trade.query";
                data.ReqUrl = "https://pay.wepayez.com/pay/gateway";
                //data.ReqUrl = "https://pay.swiftpass.cn/pay/gateway";
                data.NotifyUrl = "/PG/wechat/PayNotify";
                data.Domain = Configuration["Domain"];
                result.Succeeded = _paymentGatewayBLL.SaveOrUpdateConfig(data);
                result.Message = Resources.Message.SaveSuccess;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;

            }
            return result;
        }


        [HttpPost]
        public SystemResult SaveAliPayConfig([FromForm] AliPayConfig data)
        {
            SystemResult result = new SystemResult();
            try
            {
                data.Gateway = PaymentGateType.ALIPAY.ToString();
                data.Version = "2.0";
                data.Service = "pay.alipay.webpay.intl";
                data.QueryService = "unified.trade.query";
                data.MchId = string.IsNullOrEmpty(data.MchId) ? "" : data.MchId;
                data.Key = string.IsNullOrEmpty(data.Key) ? "" : data.Key;
                data.ReqUrl = "https://gateway.wepayez.com/pay/gateway";
                data.NotifyUrl = "/PG/alipay/PayNotify";
                data.Domain = Configuration["Domain"];
                data.CallBackUrl = "/PG/alipay/PayNotify";
                result.Succeeded = _paymentGatewayBLL.SaveOrUpdateConfig(data);
                result.Message = Resources.Message.SaveSuccess;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;

            }
            return result;
        }


        [HttpPost]
        public SystemResult SaveAliPayHKConfig([FromForm] AliPayConfig data)
        {
            SystemResult result = new SystemResult();
            try
            {
                data.Gateway = PaymentGateType.ALIPAYHK.ToString();
                data.Version = "2.0";
                data.Service = "pay.alipay.webpay.intl";
                data.QueryService = "unified.trade.query";
                data.MchId = string.IsNullOrEmpty(data.MchId) ? "" : data.MchId;
                data.Key = string.IsNullOrEmpty(data.Key) ? "" : data.Key;
                data.ReqUrl = "https://gateway.wepayez.com/pay/gateway";
                data.NotifyUrl = "/PG/alipay/PayNotify";
                data.Domain = Configuration["Domain"];
                data.CallBackUrl = "/PG/alipay/PayNotify";
                result.Succeeded = _paymentGatewayBLL.SaveOrUpdateConfig(data);
                result.Message = Resources.Message.SaveSuccess;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;

            }
            return result;
        }

        [HttpPost]
        public SystemResult SaveAtomeConfig([FromForm] AtomeConfig data)
        {
            SystemResult result = new SystemResult();
            try
            {
                data.Gateway = PaymentGateType.ATOME.ToString();
                result.Succeeded = _paymentGatewayBLL.SaveOrUpdateConfig(data);
                result.Message = Resources.Message.SaveSuccess;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;

            }
            return result;
        }

    }
}
