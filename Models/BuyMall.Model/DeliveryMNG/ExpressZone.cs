using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class ExpressZone : BaseEntity<Guid>
    {
        /// <summary>
        /// 快递公司ID
        /// </summary>
        public Guid ExpressId { get; set; }
        [MaxLength(10)]
        [Column(TypeName = "varchar")]
        public string Code { get; set; }
        public Guid NameTransId { get; set; }
        public Guid RemarkTransId { get; set; }
        /// <summary>
        /// 燃油附加費
        /// </summary>
        public decimal FuelSurcharge { get; set; }
        

    }
}