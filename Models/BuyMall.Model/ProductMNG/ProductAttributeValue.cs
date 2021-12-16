using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDMall.Model
{
    public class ProductAttributeValue : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid AttrId { get; set; }
        [Column(Order = 4, TypeName = "varchar")]
        [StringLength(20)]
        public string Code { get; set; }
        [Column(Order = 5)]
        public Guid DescTransId { get; set; }

        [Column(Order = 6, TypeName = "varchar")]
        [StringLength(300)]
        public string Image { get; set; }
        [Column(Order = 7)]
        public Guid MerchantId { get; set; }

      

    }
}
