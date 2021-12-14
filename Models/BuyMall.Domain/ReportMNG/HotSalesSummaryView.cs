using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class HotSalesSummaryView
    {
        public List<string> TitleList { get; set; } = new List<string>();

        public List<HotSalesDetailView> HotSalesDetailList { get; set; } = new List<HotSalesDetailView>();
    }
}
