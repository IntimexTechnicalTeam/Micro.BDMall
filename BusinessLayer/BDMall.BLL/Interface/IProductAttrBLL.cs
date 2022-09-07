namespace BDMall.BLL
{
    public interface IProductAttrBLL : IDependency
    {
        /// <summary>
        /// 获取产品库存属性的属性值
        /// </summary>
        /// <param name="prodId"></param>
        /// <returns></returns>
        List<AttributeObjectView> GetInvAttributeByProductMap(Guid prodId);
        /// <summary>
        /// 获取产品非库存属性的属性值
        /// </summary>
        /// <param name="prodId"></param>
        /// <returns></returns>
        List<AttributeObjectView> GetNonInvAttributeByProductMap(Guid prodId);
    }
}
