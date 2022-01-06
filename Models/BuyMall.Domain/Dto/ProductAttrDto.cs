using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductAttrDto
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        /// <summary>
        /// 產品ID
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// 屬性ID
        /// </summary>

        public Guid AttrId { get; set; }
        /// <summary>
        /// 序號
        /// </summary>

        public int Seq { get; set; }

        public bool IsInv { get; set; }

        public Guid CatalogID { get; set; }


        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }


        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }

        public List<ProductAttrValueDto> AttrValues { get; set; } = new List<ProductAttrValueDto>();

        public string AttrName { get; set; }
    }
}
