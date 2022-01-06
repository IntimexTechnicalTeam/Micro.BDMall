using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDMall.Model
{
    public class ProductAttrValue : BaseEntity<Guid>
    {

        /// <summary>
        /// 产品配对属性Id
        /// </summary>
        [Column(Order = 3)]
        public Guid ProdAttrId { get; set; }

        /// <summary>
        /// 屬性值ID
        /// </summary>
        [Column(Order = 4)]
        public Guid AttrValueId { get; set; }

        /// <summary>
        /// 序號
        /// </summary>
        [Column(Order = 5)]
        public int Seq { get; set; }

        /// <summary>
        /// 附加价钱
        /// </summary>
        [Column(Order = 6)]

        public decimal AdditionalPrice { get; set; }

        //[ForeignKey("ProdAttrId")]
        //public virtual ProductAttr ProdAttr { get; set; }

        //[ForeignKey("AttrValueId")]
        //public virtual ProductAttributeValue ProdAttrValue { get; set; }
    }
}
