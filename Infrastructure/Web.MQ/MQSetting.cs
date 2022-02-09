using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.MQ
{
    public static class MQSetting
    {     
		 /// <summary>
        /// 更新ProductQty队列
        /// </summary>
        public const string WeChatUpdateQtyQueue = "WeChat.Update.Qty.Queue";
        public const string WeChatUpdateQtyExchange = "WeChat.Update.Qty.Exchange";

        /// <summary>
        /// 购物车过期队列
        /// </summary>
        public const string WeChatShoppingCartTimeOutQueue = "WeChat.ShoppingCartTimeOut.Queue";
        public const string WeChatShoppingCartTimeOutExchange = "WeChat.ShoppingCartTimeOut.Exchange";
        public const string DelayShoppingCartTimeOutQueue = "Delay.ShoppingCartTimeOut.Queue";

        /// <summary>
        /// 支付超时队列
        /// </summary>
        public const string WeChatPayTimeOutQueue = "WeChat.PayTimeOut.Queue";
        public const string WeChatPayTimeOutExchange = "WeChat.PayTimeOut.Exchange";
        public const string DelayPayTimeOutQueue = "Delay.PayTimeOut.Queue";
    }
}
