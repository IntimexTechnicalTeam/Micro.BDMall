using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        [MaxLength(100)]
        [Column(Order = 1,TypeName ="nvarchar")]
        public string ProductName { get; set; }

        public DateTime  CreateDate { get; set; }

        [MaxLength(50)]
        [Column(Order = 3, TypeName = "nvarchar")]
        public string CreateBy { get; set; }

        public string Remark { get; set; }

    }


}
