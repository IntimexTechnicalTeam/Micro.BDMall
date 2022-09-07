namespace BDMall.Domain
{
    public class AliPayConfig : PayConfig
    {
        public string Version { get; set; }
        public string Service { get; set; }
        public string MchId { get; set; }
        public string Key { get; set; }
        public string Domain { get; set; }
        public string MchName { get; set; }
        public string NotifyUrl { get; set; }
        public string ReqUrl { get; set; }
        public string QueryService { get; set; }
        public string CallBackUrl { get; set; }
    }
}
