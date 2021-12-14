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
        /// 
        /// </summary>
        public const string ViewBDPageQueue = "View.BDPage.Queue";

        /// <summary>
        /// 
        /// </summary>
        public const string ViewBDPageExchange = "View.BDPage.Exchange";
		
		 /// <summary>
        /// 更新库存队列
        /// </summary>
        public const string UpdateQtyQueue = "Update.Qty.Queue";

        /// <summary>
        /// 
        /// </summary>
        public const string UpdateQtyExchange = "Update.Qty.Exchange";

        /// <summary>
        /// 处理同步积分队列
        /// </summary>
        public const string TransPointsDataQueue = "Trans.PointsData.Queue";

        /// <summary>
        /// 
        /// </summary>
        public const string TransPointsDataExchange = "Trans.PointsData.Exchange";


        /// <summary>
        /// 处理积分兑换队列
        /// </summary>
        public const string TransinDealFunQueue = "Transin.DealFun.Queue";

        /// <summary>
        /// 
        /// </summary>
        public const string TransinDealFunExchange = "Transin.DealFun.Exchange";

        /// <summary>
        /// 处理更新产品限时价格延时队列
        /// </summary>
        public const string DelayUpdateProductLimitPriceQueue = "Delay.Update.Product.LimitPrice.Queue";

        /// <summary>
        /// 处理更新产品限时价格队列
        /// </summary>
        public const string UpdateProductLimitPriceQueue = "Update.Product.LimitPrice.Queue";

        /// <summary>
        /// 
        /// </summary>
        public const string UpdateProductLimitPriceExchange= "Update.Product.LimitPrice.Exchange";


    }
}
