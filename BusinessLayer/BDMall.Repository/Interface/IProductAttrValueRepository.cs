namespace BDMall.Repository
{
    public interface IProductAttrValueRepository:IDependency
    {
        bool CheckHasInvRecordByAttrValueId(Guid attrValueId);
    }
}
