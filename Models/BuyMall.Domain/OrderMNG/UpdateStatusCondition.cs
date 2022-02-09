using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class UpdateStatusCondition
    {
        public Guid OrderId { get; set; }


        public OrderStatus CurrentStatus { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public OrderStatus Status { get; set; }
        /// <summary>
        /// 倉庫Id
        /// </summary>
        //public Guid LocationId { get; set; }
        /// <summary>
        /// 快遞單號
        /// </summary>
        //public string TrackingNo { get; set; }

        /// <summary>
        /// 送貨單快遞單號信息
        /// </summary>
        public List<DeliveryTrackingInfo> DeliveryTrackingInfo { get; set; } = new List<DeliveryTrackingInfo> { new DeliveryTrackingInfo { } };

        /// <summary>
        /// 是否自动发货记录
        /// </summary>
        public bool IsMassProcessRecord { get; set; }
    }
}
