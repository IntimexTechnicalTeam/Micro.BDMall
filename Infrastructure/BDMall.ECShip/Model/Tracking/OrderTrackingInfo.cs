using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.ECShip.Model.Tracking
{
    public class OrderTrackingInfo
    {
        public string Message { get; set; }

        public string TrackingNo { get; set; }

        public string Status { get; set; }

        public List<TrackingInfo> TrackingInfos { get; set; }

    }
}
