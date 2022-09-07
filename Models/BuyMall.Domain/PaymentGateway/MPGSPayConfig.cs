namespace BDMall.Domain
{
    public class MPGSPayConfig : PayConfig
    {
        public MPGSPayConfig()
        {
            Debug = false;
            UseProxy = false;
            UseSSL = true;
            IgnoreSslErrors = false;
            IsPassOnLocal = true;
        }
        public bool Debug { get; set; }

        public bool UseProxy { get; set; }


        public string ProxyHost { get; set; }

        public string ProxyUser { get; set; }
        public string ProxyPassword { get; set; }
        public string ProxyDomain { get; set; }

        public bool UseSSL { get; set; }
        public bool IgnoreSslErrors { get; set; }

        public bool IsPassOnLocal { get; set; }

        public string MerchantName { get; set; }
        public string GatewayHost { get; set; }
        public string Version { get; set; }
        public string MerchantId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }

    public enum MPGSPayConfigKey
    {
        MerchantId,
        Key,
        Version,
        MerchantName,
        Domain,
        NotifyUrl,
        ReqUrl,
        Service
    }
}
