namespace BDMall.Repository
{
    public interface IWarehouseRepository:IDependency
    {
        List<Warehouse> GetWarehouseList(WarehouseDto cond);
    }
}
