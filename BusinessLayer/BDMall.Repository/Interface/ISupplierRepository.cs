namespace BDMall.Repository
{
    public interface ISupplierRepository:IDependency
    {
        List<Supplier> GetSupplierList(Supplier cond);
    }
}
