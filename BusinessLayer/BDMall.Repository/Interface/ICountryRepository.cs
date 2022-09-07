namespace BDMall.Repository
{
    public interface ICountryRepository:IDependency
    {
        List<KeyValue> GetList(Language lang);
    }
}
