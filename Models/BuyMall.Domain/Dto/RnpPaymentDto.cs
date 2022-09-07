namespace BDMall.Domain
{
    public class RnpPaymentDto
    {
        public Guid Id { get; set; }
        public string BankAccount { get; set; }

        public Guid FormId { get; set; }

        public string PayGateway { get; set; }

    }
}
