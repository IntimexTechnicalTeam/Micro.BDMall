namespace BDMall.Domain
{
    public class RnpSubmitDto
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }

        public decimal? TotalAmount { get; set; }

        public bool IsPayed { get; set; }

        public string GatewayResponse { get; set; }

        public string UpdatePayedBy { get; set; }

        public string Signature { get; set; }

        public List<RnpSubmitDataInfoView> RnpSubmitDataInfo { get; set; }
        public List<RnpSubmitDataDto> RnpSubmitDatas { get; set; }
    }
}
