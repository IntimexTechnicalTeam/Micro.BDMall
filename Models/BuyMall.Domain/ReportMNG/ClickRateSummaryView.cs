using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ClickRateSummaryView
    {
        public List<string> TitleList { get; set; } = new List<string>();

        public List<ClickRateDetailView> ClickRateDetailList { get; set; } = new List<ClickRateDetailView>();
    }
}
