using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Enums
{
    public enum PreHotType
    {
        Hot_PromotionView = 1,
        Hot_PromotionBanner = 2,
        Hot_PromotionMerchant = 3,
        Hot_PromotionProduct = 4,

        /// <summary>
        /// 具体的商品预热Key
        /// </summary>
        Hot_Products = 5,

        Hot_ProductImage = 6,

        /// <summary>
        /// 具体的商家预热Key
        /// </summary>
        Hot_Merchants = 8,

        Hot_SearchKey = 9,

        Hot_HeaderMenu = 10,

        Hot_FooterMenu = 11,

        Hot_MerchantFreeCharge = 12,

        Favorite = 13,

        Hot_PreProductStatistics = 14
    }
}
