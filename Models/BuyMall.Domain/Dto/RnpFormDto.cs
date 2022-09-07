namespace BDMall.Domain
{
    public class RnpFormDto 
    {
        public RnpFormDto()
        {
            RnpSubmits = new List<RnpSubmitDto>();
            RnpQuestions = new List<RnpQuestionDto>();
            RnpPayments = new List<RnpPaymentDto>();
        }
        public string Title { get; set; }
        public string DescTop { get; set; }
        public string DescBottom { get; set; }

        public bool IsPayment { get; set; }

        public int? CurrencyId { get; set; }

        public bool IsLimit { get; set; }

        public int? Limit { get; set; }

        public bool IsNotice { get; set; }

        public string NoticeEmail { get; set; }

        public bool IsSign { get; set; }

        public new bool IsDeleted { get; set; }

        public Guid? CopyFormId { get; set; }

        public Guid? RootFormId { get; set; }
        public List<RnpSubmitDto> RnpSubmits { get; set; }

        public List<RnpQuestionDto> RnpQuestions { get; set; }

        public List<RnpPaymentDto> RnpPayments { get; set; }
    }
}
