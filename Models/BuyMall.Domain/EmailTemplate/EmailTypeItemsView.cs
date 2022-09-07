namespace BDMall.Domain
{
    public class EmailTypeItemsView : BaseDto
    {
        public MailType EmailType { get; set; }

        public string EmailTypeId { get; set; }

        public string Description { get; set; }

        public List<Guid> EmailItems { get; set; }
    }
}
