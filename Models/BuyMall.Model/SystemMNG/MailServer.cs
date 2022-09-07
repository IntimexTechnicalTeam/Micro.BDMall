namespace BDMall.Model
{
    public class MailServer : BaseEntity<Guid>
    {
        public string Code { get; set; }

        public string Server { get; set; }

        public string Port { get; set; }

        public bool IsSSL { get; set; }
    }
}
