using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BDMall.Model
{
    public class ProductRelatedItem : BaseEntity<Guid>
    {
        /// <summary>
        /// 產品Id
        /// </summary>
        [Column(Order = 3)]
        public Guid ProductId { get; set; }


        /// <summary>
        /// 相關產品ID
        /// </summary>
        [Column(Order = 4)]
        public Guid ItemId { get; set; }

        /// <summary>
        /// 相关产品Code
        /// </summary>
        [StringLength(100)]
        [Column(Order = 5, TypeName = "varchar")]
        public string ItemCode { get; set; }

        /// <summary>
        /// 序號
        /// </summary>
        [Column(Order = 6)]
        public int Seq { get; set; }

        //[ForeignKey("ProductId")]
        //public virtual Product Product { get; set; }
    }
}
