namespace BDMall.BLL
{
    public interface IShoppingCartBLL:IDependency
    {
        Task<ShopCartInfo> GetShoppingCart();

        Task<SystemResult> AddtoCartAsync(CartItem cartItem);

        Task<SystemResult> UpdateCartItemAsync(Guid itemId, int qty);

        Task<SystemResult> RemoveFromCart(Guid itemId);

        Task<SystemResult> ClearShoppingCart();

        Task<SystemResult> RemoveFromCart(ShoppingCartItem shoppingCart);
    }

}
