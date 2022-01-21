using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class MemberGroupDiscountItem : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid DiscountId { get; set; }

        [Column(Order = 4)]
        public Guid MemberGroupId { get; set; }
    }
}
