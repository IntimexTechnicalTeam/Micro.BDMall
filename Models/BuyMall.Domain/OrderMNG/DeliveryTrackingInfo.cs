namespace BDMall.Domain
{
    public class DeliveryTrackingInfo
    {
        /// <summary>
        /// Delivery Id
        /// </summary>
        public Guid Id { get; set; }

        public string TrackingNo { get; set; }

        public string ECShipNo { get; set; }

        public string ECShipDocNo { get; set; }
        public Guid LocationId { get; set; }
    }
}
