namespace BDMall.Model
{
    public class OrderMassProcessStatus : BaseEntity<Guid>
    {
        [Column(Order = 3)]
        public Guid DeliveryId { get; set; }

        [Column(Order = 4)]
        public MassProcessStatusType Status { get; set; }

        [Column(Order = 5, TypeName = "NVarchar")]
        [MaxLength(2000)]
        public string Message { get; set; }
    }
}
