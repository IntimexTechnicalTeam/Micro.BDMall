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
    public class ProductSpecification : BaseEntity<Guid>
    {

        [Key]
        [ForeignKey("Product")]
        public new Guid Id { get; set; }

        /// <summary>
        /// 產品参数
        /// </summary>
        [Column(Order = 3)]
        public ProductDimensionParameter ProductDimension { get; set; }


        /// <summary>
        /// 包装参数
        /// </summary>
        [Column(Order = 4)]
        public ProductPackageParameter ProductPackage { get; set; }

        /// <summary>
        /// 包裝描述
        /// </summary>
        [Column(Order = 5)]
        [StringLength(1000)]
        public string PackageDescription { get; set; }

        /// <summary>
        /// 毛重
        /// </summary>
        [Column(Order = 6)]
        public decimal GrossWeight { get; set; }


        /// <summary>
        /// 淨重
        /// </summary>
        [Column(Order = 7)]
        public decimal NetWeight { get; set; }

        /// <summary>
        /// 重量單位
        /// </summary>
        [Column(Order = 8)]
        public WeightUnit WeightUnit { get; set; }


        public virtual Product Product { get; set; }
    }
}
