namespace BDMall.Repository
{
    public interface IShoppingCartRepository:IDependency
    {
        /// <summary>
        /// 生成Detail
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ShoppingCartItemDetailDto GetItemDetail(ShoppingCartItem item);
    }
}
