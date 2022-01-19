using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.ECShip.Model.MailTracking
{
    public class MailTrackingInfo
    {
        public int ItemCount { get; set; }

        public string Status { get; set; }

        public string DestCountryCode { get; set; }

        public string DetailFlag { get; set; }

        public string HKPItemNo { get; set; }

        public string ItemNo { get; set; }

        public string IoltType { get; set; }

        public string ItemStatusCode { get; set; }

        public string DesctCountryName { get; set; }

        public string DropOff { get; set; }

        public string ItemType { get; set; }

        public List<MailTrackingDetail> Details { get; set; }

        public string MilestoneCode { get; set; }

        public string OriginalCountryCode { get; set; }

        public string PostingDate { get; set; }

        public string PostingTime { get; set; }

        public string ServiceCode { get; set; }

        public string MilestoneDesc { get; set; }

        public string OriginalCountry { get; set; }




    }
}
