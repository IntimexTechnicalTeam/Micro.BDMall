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
        public Guid Id { get; set; }

        public string CountryCode { get; set; }
        public string ShipCode { get; set; }
        public string ServiceType { get; set; }
        public Guid ExpressCompanyId { get; set; }
        public string ExpressCompanyName { get; set; }
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
        public string Vcode { get; set; }
    }
}
