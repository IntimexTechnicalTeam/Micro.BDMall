using BDMall.Domain;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

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
