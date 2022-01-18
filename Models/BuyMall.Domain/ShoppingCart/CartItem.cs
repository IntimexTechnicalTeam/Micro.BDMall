using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace BDMall.Domain
{
    public class CartItem
    {
        public Guid Sku { get; set; }
        public Guid ProductId { get; set; }
        public string ProdCode { get; set; }
        public Guid Attr1 { get; set; } = Guid.Empty;

        public Guid Attr2 { get; set; } = Guid.Empty;

        public Guid Attr3 { get; set; } = Guid.Empty;

        public string AttrName1 { get; set; }

        public string AttrName2 { get; set; }

        public string AttrName3 { get; set; }

        public string AttrTypeName1 { get; set; }

        public string AttrTypeName2 { get; set; }

        public string AttrTypeName3 { get; set; }

        /// <summary>
        /// 非必填字段
        /// </summary>
        public Guid MemberId { get; set; }

        public DateTime CreateAt { get; set; }

        /// <summary>
        /// 购物数量
        /// </summary>
        [DataMember]
        [Required(ErrorMessage = "购物数量必填")]
        [MinLength(1)]
        public int Qty { get; set; }

        /// <summary>
        /// 增量
        /// </summary>
        public int AddQty { get; set; }
    }
}
