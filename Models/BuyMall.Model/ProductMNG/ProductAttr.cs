using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace BDMall.Model
{
    public class ProductAttr : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        /// <summary>
        /// 產品ID
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// 屬性ID
        /// </summary>
        [Column(Order = 4)]
        public Guid AttrId { get; set; }
        /// <summary>
        /// 序號
        /// </summary>
        [Column(Order = 5)]
        public int Seq { get; set; }
        [Column(Order = 6)]
        public bool IsInv { get; set; }
        [Column(Order = 7)]
        public Guid CatalogID { get; set; }
        
       
    }
}
