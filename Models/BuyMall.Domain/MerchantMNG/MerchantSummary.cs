using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MerchantSummary
    {
        /// <summary>
        /// 商家Id
        /// </summary>
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }
        /// <summary>
        /// 商家編碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 評分
        /// </summary>
        public decimal Score { get; set; }
        /// <summary>
        /// 是否被收藏
        /// </summary>
        public bool IsFavorite { get; set; }
        /// <summary>
        /// 小圖標
        /// </summary>
        public string LogoS { get; set; }
        /// <summary>
        /// 大圖標
        /// </summary>
        public string LogoB { get; set; }

        /// <summary>
        /// 商店條款
        /// </summary>
        public string MerchantTerms { get; set; }

        /// <summary>
        /// 送貨/退貨條款
        /// </summary>
        public string ReturnTerms { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ProdouctListSummaryView> Products { get; set; } = new List<ProdouctListSummaryView>();

        /// <summary>
        /// 是否香港品牌
        /// </summary>
        public bool IsHongKong { get; set; }

    }
}
