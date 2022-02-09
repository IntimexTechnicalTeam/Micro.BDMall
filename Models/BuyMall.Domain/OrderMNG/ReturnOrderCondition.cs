using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain
{
    public class ReturnOrderCondition
    {
        public PageInfo PageInfo { get; set; }
        public Guid ROrderId { get; set; }
        public string RONoFrom { get; set; }
        public string RONoTo { get; set; }
        //public string OrderNoFrom { get; set; }
        //public string OrderNoTo { get; set; }
        public string OrderNo { get; set; }
        public string CreateDateFrom { get; set; }
        public string CreateDateTo { get; set; }
        public Guid MemberId { get; set; }
        public Guid MerchantId { get; set; }
        public Guid OrderId { get; set; }
        public Guid OrderDeliveryId { get; set; }
        public Guid SkuId { get; set; }
        public ReturnOrderStatus StatusCode { get; set; }
    }
}
