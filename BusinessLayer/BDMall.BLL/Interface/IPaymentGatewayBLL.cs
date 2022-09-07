namespace BDMall.BLL
{
    public interface IPaymentGatewayBLL : IDependency
    {
        PayConfig GetConfig(PaymentGateType gateway);

        bool SaveOrUpdateConfig(PayConfig config);
    }
}
