using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ExpressChargeInfo
    {
        public Guid Id { get; set; } = Guid.Empty;

        public string CountryCode { get; set; } = string.Empty;
        public string ShipCode { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public Guid ExpressCompanyId { get; set; } =Guid.Empty;
        public string ExpressCompanyName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPrice
        {
            get
            {
                var disPrice = Price - Discount;
                if (disPrice < 0)
                {
                    disPrice = 0;
                }
                return disPrice;
            }
        }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountOriginalPrice
        {
            get
            {
                var disPrice = OriginalPrice - Discount;
                if (disPrice < 0)
                {
                    disPrice = 0;
                }
                return disPrice;
            }
        }
        /// <summary>
        /// 校驗碼
        /// </summary>
        public string Vcode { get; set; } = string.Empty;
    }
}
