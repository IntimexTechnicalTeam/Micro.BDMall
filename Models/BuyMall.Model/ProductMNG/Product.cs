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
    public class Product : BaseEntity<Guid>
    {

        [Column(Order = 3)]
        public Guid MerchantId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 4)]
        public string Code { get; set; }

        [Column(Order = 5)]
        public Guid NameTransId { get; set; }

        [Column(Order = 6)]
        public Guid TitleTransId { get; set; }


        [Column(Order = 7)]
        public Guid KeyWordTransId { get; set; }


        [Column(Order = 8)]
        public Guid SeoDescTransId { get; set; }


        [Column(Order = 9)]
        public Guid IntroductionTransId { get; set; }


        [Column(Order = 10)]
        public Guid DetailTransId { get; set; }


        [Required]
        [Column(Order = 11)]
        public Guid CatalogId { get; set; }



        //[Required]
        //[Column(Order = 12)]
        //public string CatalogPath { get; set; }

        /// <summary>
        /// 顯示前臺價格
        /// </summary>
        [Required]
        [Column(Order = 12)]
        public decimal SalePrice { get; set; }

        [Required]
        [Column(Order = 13)]
        public decimal OriginalPrice { get; set; }


        [Required(AllowEmptyStrings = true)]
        [Column(Order = 14, TypeName = "varchar")]
        [StringLength(20)]
        public string CurrencyCode { get; set; }


        /// <summary>
        /// 默認顯示的圖片
        /// </summary>

        [Column(Order = 15)]
        public Guid DefaultImage { get; set; }

        /// <summary>
        /// 是否已审批
        /// </summary>
        [Column(Order = 16)]
        public bool IsApprove { get; set; }

        /// <summary>
        /// 内部价格
        /// </summary>
        [Column(Order = 17)]
        public decimal InternalPrice { get; set; }

        //[Column(Order = 18)]
        //public int VisitCounter { get; set; }

        //[Column(Order = 19)]
        //public int PurchaseCounter { get; set; }
        [Column(Order = 18)]
        [StringLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 产品有效开始时间
        /// </summary>
        [Column(Order = 19)]
        public DateTime? ActiveTimeFrom { get; set; }
        /// <summary>
        /// 产品有效开始时间
        /// </summary>
        [Column(Order = 20)]
        public DateTime? ActiveTimeTo { get; set; }

        [Column(Order = 21)]
        public Guid FromProductId { get; set; }

        [Column(Order = 22)]
        public decimal MarkUpPrice { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        [Column(Order = 23)]
        public ProductStatus Status { get; set; }

        /// <summary>
        /// 成分ID
        /// </summary>
        [Column(Order = 24)]
        public Guid IngredientTransId { get; set; }

        /// <summary>
        /// 使用说明ID
        /// </summary>
        [Column(Order = 25)]
        public Guid InstructionsTransId { get; set; }

        /// <summary>
        /// 時段價格--現在改爲以前SalePrice功能，主要是修改後臺價格
        /// </summary>
        [Column(Order = 26)]
        public decimal TimePrice { get; set; }

        //[Column(Order = 97)]
        //public new bool? IsActive { get; set; }



    }
}
