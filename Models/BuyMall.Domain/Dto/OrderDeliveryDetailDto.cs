using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class OrderDeliveryDetailDto :BaseDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid OrderId
        {
            get; set;
        }

        /// <summary>
        /// 送货单ID
        /// </summary>
        public Guid DeliveryId
        {
            get; set;
        }

        /// <summary>
        /// 单品ID
        /// </summary>
        public Guid SkuId
        {
            get; set;
        }
        /// <summary>
        /// 產品Id
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Qty
        {
            get; set;
        }

        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal OriginalPrice
        {
            get; set;
        }

        /// <summary>
        /// 购买价格
        /// </summary>
        public decimal SalePrice
        {
            get; set;
        }

        /// <summary>
        /// 是否贈品
        /// </summary>
        public bool IsFree { get; set; }

        /// <summary>
        /// 多件購買優惠價錢
        /// </summary>
        public decimal SetPrice { get; set; }

        /// <summary>
        /// 每件產品實際支付價錢
        /// </summary>
        public decimal PayPrice { get; set; }

        /// <summary>
        /// 推廣規則ID
        /// </summary>
        public Guid RuleId { get; set; }

        /// <summary>
        /// 是否有赠品
        /// </summary>
        public bool HasFree { get; set; }

        /// <summary>
        /// 倉庫Id
        /// </summary>
        public Guid LocationId { get; set; }


        /// <summary>
        /// 產品對應的快遞單號
        /// </summary>
       
        public string TrackingNo { get; set; }


        /// <summary>
        /// 附加价钱1
        /// </summary>
       
        public decimal AddPrice1 { get; set; }
        /// <summary>
        /// 附加价钱2
        /// </summary>
      
        public decimal AddPrice2 { get; set; }
        /// <summary>
        /// 附加价钱3
        /// </summary>

        public decimal AddPrice3 { get; set; }
    }
}
