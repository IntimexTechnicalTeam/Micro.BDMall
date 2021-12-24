using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class MerchantPromotionBannerView
    {

        public Guid Id { get; set; }

        public Guid PromotionId { get; set; }

        public Language Lang { get; set; }

        public string Language { get; set; }


        public string Image { get; set; }

        public int Seq { get; set; }

        /// <summary>
        /// 是否打開新窗口
        /// </summary>
        public bool IsOpenWindow { get; set; }

        public bool IsDelete { get; set; }

        public string BannerLink { get; set; }
    }
}
