using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BDMall.Enums;

namespace BDMall.Model
{
    public class MerchantPromotionBanner : BaseEntity<Guid>
    {

        [Required]
        [Column(Order = 4)]
        public Guid PromotionId { get; set; }

        /// <summary>
        /// Banner图片路径
        /// </summary>
        [MaxLength(200)]
        [Column(TypeName = "varchar", Order = 5)]
        public string Image { get; set; }

        [Column(Order = 6)]
        public Language Language { get; set; }

        [Column(Order = 7)]
        public int Seq { get; set; }

        [Column(Order = 8)]
        public string BannerLink { get; set; }

        [Column(Order = 9)]
        public bool IsOpenWindow { get; set; }

        //[ForeignKey("PromotionId")]
        //public virtual MerchantPromotion MerchantPromotion { get; set; }


    }
}
