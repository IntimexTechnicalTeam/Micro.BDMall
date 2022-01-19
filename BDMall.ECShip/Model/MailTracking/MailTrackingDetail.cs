using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.ECShip.Model.MailTracking
{
    public class MailTrackingDetail
    {
        public string EventCode { get; set; }

        public string ExtraMessageCode { get; set; }

        public string LocationCountryCode { get; set; }

        public string MessageCode { get; set; }

        public string ExtraMessage { get; set; }

        public string Message { get; set; }

        public string MessageLocation { get; set; }

        public string MessageTime { get; set; }

        public int Seq { get; set; }
    }
}
