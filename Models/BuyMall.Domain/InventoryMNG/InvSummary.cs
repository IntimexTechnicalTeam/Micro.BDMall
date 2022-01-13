using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class InvSummary: InvSummaryView
    {
        public int HoldTotalQty { get; set; }
        public List<InvSummaryDetl> InventoryDetailList { get; set; } = new List<InvSummaryDetl>();
    }
}
