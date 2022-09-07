namespace BDMall.Domain
{
    public class StripePayConfig : PayConfig
    {
        public string StripeSecretKey { get; set; }

        public string StripePublishableKey { get; set; }


    }
}
