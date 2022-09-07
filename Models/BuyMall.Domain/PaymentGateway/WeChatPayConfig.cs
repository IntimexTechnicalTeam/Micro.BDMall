namespace BDMall.Domain
{
    public class WeChatPayConfig : PayConfig
    {
        public string MerchantId { get; set; }

        public string Key { get; set; }


        public string Version { get; set; }

        public string MerchantName { get; set; }
        public string Domain { get; set; }
        public string NotifyUrl { get; set; }

        public string ReqUrl { get; set; }
        public string Service { get; set; }
        public string QueryService { get; set; }

    }
}
