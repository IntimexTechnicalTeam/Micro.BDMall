using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDMall.Model
{
    public class MerchantECShipInfo : BaseEntity<Guid>
    {

        [Key]
        [ForeignKey("Merchant")]
        public new Guid Id { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "varchar", Order = 4)]
        public string SPName { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 5)]
        public string SPPassword { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "varchar", Order = 6)]
        public string SPIntegraterName { get; set; }


        [MaxLength(20)]
        [Column(TypeName = "varchar", Order = 7)]
        public string ECShipName { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 8)]
        public string ECShipPassword { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "varchar", Order = 9)]
        public string ECShipIntegraterName { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar", Order = 10)]
        public string ECShipEmail { get; set; }

    }
}
