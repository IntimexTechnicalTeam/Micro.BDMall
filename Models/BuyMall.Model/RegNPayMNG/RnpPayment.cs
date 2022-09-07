namespace BDMall.Model
{
    public class RnpPayment : BaseEntity<Guid>
    {
        [Required]
        [StringLength(50)]
        public string BankAccount { get; set; }

        public Guid FormId { get; set; }

        [Required]
        [StringLength(10)]
        public string PayGateway { get; set; }      
    }
}
