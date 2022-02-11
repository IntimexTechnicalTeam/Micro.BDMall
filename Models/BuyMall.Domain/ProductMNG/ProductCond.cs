using System;
using System.ComponentModel;
using Web.Framework;

namespace BDMall.Domain
{
    public class ProductCond : PageInfo
    {
      
        /// <summary>
        /// 商家Id
        /// </summary>
       
        public Guid MerchantId { get; set; } = Guid.Empty;

        /// <summary>
        /// 商品名称
        /// </summary>      
        public string ProductName { get; set; } = "";

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; } = "";

        /// <summary>
        /// 商品Code
        /// </summary>
        public string ProductCode { get; set; } = "";

        public string OrderBy { get; set; } = "New";
    }
}
