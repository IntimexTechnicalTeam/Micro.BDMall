namespace BDMall.Domain
{
    public class MailServerInfo
    {
        public Guid Id { get; set; }
        public String Code { get; set; }
        public String MailServer { get; set; }
        public String Port { get; set; }
        public bool IsSSL { get; set; }
    }
}
