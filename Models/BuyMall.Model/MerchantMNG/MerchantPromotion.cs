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
    public class MerchantPromotion : BaseEntity<Guid>
    {
        [Required]
        [Column(Order = 3)]
        public Guid MerchantId { get; set; }


        [Column(Order = 4)]
        public Guid DescTransId { get; set; }

        [Column(Order = 5)]
        public Guid IntorductionTranId { get; set; }
        [Column(Order = 6)]
        public Guid NameTranId { get; set; }



        [Column(Order = 7)]
        public Guid CoverId { get; set; }

        [Column(Order = 8)]
        public Guid SmallLogoId { get; set; }

        [Column(Order = 9)]
        public Guid BigLogoId { get; set; }


        [Column(Order = 10)]
        public ApproveType ApproveStatus { get; set; }


        [Column(Order = 11)]
        public Guid TAndCTranId { get; set; }

        [Column(Order = 12)]
        public Guid NoticeTranId { get; set; }

        /// <summary>
        /// 退換貨條款
        /// </summary>
        [Column(Order = 13)]
        public Guid ReturnTermsTranId { get; set; }

        /// <summary>
        /// 商戶訂單相關
        /// </summary>
        [Column(Order = 14)]
        public Guid OrderTransId { get; set; }

        /// <summary>
        /// 本地冷靜期
        /// </summary>
        [Column(Order = 15)]
        public int LocalCoolDownDay { get; set; } = 7;

        /// <summary>
        /// 海外冷靜期
        /// </summary>
        [Column(Order = 16)]
        public int OverSeaCoolDownDay { get; set; } = 7;
    }
}
