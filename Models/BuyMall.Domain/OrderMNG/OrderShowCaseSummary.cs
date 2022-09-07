namespace BDMall.Domain
{
    public class OrderShowCaseSummary
    {
        public Guid Id { get; set; }
        public string OrderNO { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateDateString { get; set; }
        public string UpdateDateString { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
