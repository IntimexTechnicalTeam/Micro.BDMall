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
    public class ProductExtension : BaseEntity<Guid>
    {
        /// <summary>
        /// 產品ID
        /// </summary>
        [Key]
        [ForeignKey("Product")]
        public new Guid Id { get; set; }
        public virtual Product Product { get; set; }

        [Column(Order = 3)]
        /// <summary>
        /// 產品瀏覽層次
        /// </summary>
        public int PermissionLevel { get; set; }

        [Column(Order = 4)]
        /// <summary>
        /// 是否上架
        /// </summary>
        public bool IsOnSale { get; set; }
        //public bool Standard { get; set; }

        /// <summary>
        /// 是否組合產品
        /// </summary>
        //public bool IsComb { get; set; }
        [Column(Order = 5)]

        /// <summary>
        /// 是否售罄
        /// </summary>
        public bool IsSaleOff { get; set; }

        /// <summary>
        /// 最小购买数量
        /// </summary>
        [Column(Order = 6)]
        public int MinPurQty { get; set; }

        /// <summary>
        /// 最大购买数量
        /// </summary>
        [Column(Order = 7)]
        public int MaxPurQty { get; set; }

        [StringLength(1000)]
        [Column(Order = 8)]
        public string YoutubeLink { get; set; }

        /// <summary>
        /// 产品类型，热销产品、新产品、免邮费产品
        /// </summary>
        [Column(Order = 9)]
        public ProductType ProductType { get; set; }

        [Column(Order = 10, TypeName = "varchar")]
        [StringLength(20)]
        public string Gtin { get; set; }

        [Column(Order = 11)]
        public int SalesQuota { get; set; }

        [Column(Order = 12)]
        public int SafetyStock { get; set; }

        [Column(Order = 13)]
        public bool IsLimit { get; set; }

        [Column(Order = 14)]
        public bool IsSalesReturn { get; set; }

        [StringLength(1000)]
        [Column(Order = 15)]
        public string YoukuLink { get; set; }

        [Column(Order = 16)]
        public GS1Status GS1Status { get; set; }

        /// <summary>
        /// 清關編號
        /// </summary>
        [Column(Order = 17, TypeName = "varchar")]
        [StringLength(50)]
        public string HSCode { get; set; }
    }
}
