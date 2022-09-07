namespace BDMall.BLL
{
    public class PaymentGatewayBLL : BaseBLL, IPaymentGatewayBLL
    {

        public PaymentGatewayBLL(IServiceProvider services) : base(services)
        {
        }

        public PayConfig GetConfig(PaymentGateType gateway)
        {
            PayConfig payConfig = null;
            var settings = UnitOfWork.DataContext.Customizations.Where(d => d.Type == gateway.ToString() && d.IsActive && d.IsDeleted == false)
                .Select(d => new { Key = d.Key, Value = d.Value }).ToDictionary(d => d.Key, d => (object)d.Value);

            switch (gateway)
            {
                case PaymentGateType.WECHAT:
                    payConfig = new WeChatPayConfig();
                    payConfig.Gateway = PaymentGateType.WECHAT.ToString();
                    ClassUtility.SetValue<PayConfig, object>(payConfig, settings);
                    break;

                case PaymentGateType.MPGS1:
                    payConfig = new MPGSPayConfig();
                    payConfig.Gateway = PaymentGateType.MPGS1.ToString();
                    ClassUtility.SetValue<PayConfig, string>(payConfig, settings);
                    break;
                case PaymentGateType.MPGS2:
                    payConfig = new MPGSPayConfig();
                    payConfig.Gateway = PaymentGateType.MPGS2.ToString();
                    ClassUtility.SetValue<PayConfig, string>(payConfig, settings);
                    break;
                case PaymentGateType.STRIPE:
                    payConfig = new StripePayConfig();
                    payConfig.Gateway = PaymentGateType.STRIPE.ToString();
                    ClassUtility.SetValue<PayConfig, string>(payConfig, settings);
                    break;
                case PaymentGateType.PAYPAL:
                    payConfig = new PaypalConfig();
                    payConfig.Gateway = PaymentGateType.PAYPAL.ToString();
                    ClassUtility.SetValue<PayConfig, string>(payConfig, settings);
                    break;
                case PaymentGateType.PAYME:
                    payConfig = new PaymeConfig();
                    payConfig.Gateway = PaymentGateType.PAYME.ToString();
                    ClassUtility.SetValue<PayConfig, string>(payConfig, settings);
                    break;
                case PaymentGateType.ALIPAY:
                    payConfig = new AliPayConfig();
                    payConfig.Gateway = PaymentGateType.ALIPAY.ToString();
                    ClassUtility.SetValue<PayConfig, string>(payConfig, settings);
                    break;
                case PaymentGateType.ALIPAYHK:
                    payConfig = new AliPayConfig();
                    payConfig.Gateway = PaymentGateType.ALIPAYHK.ToString();
                    ClassUtility.SetValue<PayConfig, string>(payConfig, settings);
                    break;
                case PaymentGateType.ATOME:
                    payConfig = new AtomeConfig();
                    payConfig.Gateway = PaymentGateType.ATOME.ToString();
                    ClassUtility.SetValue<PayConfig, string>(payConfig, settings);
                    break;
            }
            return payConfig;
        }


        public bool SaveOrUpdateConfig(PayConfig config)
        {

            Type targetType = config.GetType();
            PropertyInfo[] targetProperties = targetType.GetProperties();
            List<Customization> cms = new List<Customization>();
            foreach (var e in targetProperties)
            {
                Customization cm = GenBaseConfigSetting(config.Gateway.ToString(), e.Name);
                object value = e.GetValue(config);
                ClassUtility.SetValue(cm, "Value", value ?? "");
                if (cm.Id == 0)
                {
                    cm.ClientId = Guid.Empty;
                    cm.CreateDate = DateTime.Now;
                    cm.CreateBy = Guid.Parse(CurrentUser.UserId);
                    cm.UpdateBy = Guid.Parse(CurrentUser.UserId);

                    cms.Add(cm);
                }
            }

            baseRepository.Insert(cms);

            return true;

        }
        private Customization GenBaseConfigSetting(string gateway, string key)
        {

            Customization cm1 = UnitOfWork.DataContext.Customizations.FirstOrDefault(d => d.Type == gateway && d.Key == key);
            if (cm1 == null)
            {
                cm1 = new Customization();
            }
            cm1.Type = gateway.ToString();
            cm1.Key = key.ToString();
            cm1.IsActive = true;
            cm1.IsDeleted = false;
            cm1.CreateBy = Guid.Parse(CurrentUser.UserId);
            cm1.CreateDate = DateTime.Now;
            cm1.UpdateBy = Guid.Parse(CurrentUser.UserId);
            cm1.UpdateDate = DateTime.Now;
            return cm1;
        }
    }
}
