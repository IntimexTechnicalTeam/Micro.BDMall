namespace BDMall.Repository
{
    public interface IProductDetailRepository:IDependency
    {
        public List<MutiLanguage> GetMutiLanguage(Guid transId);
    }
}
