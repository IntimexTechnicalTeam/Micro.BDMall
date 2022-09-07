namespace BDMall.BLL
{
    public class LowQtyEmailRequest : INotification
    {
        public List<Guid> skuList { get; set; }= new List<Guid>();
    }
}
