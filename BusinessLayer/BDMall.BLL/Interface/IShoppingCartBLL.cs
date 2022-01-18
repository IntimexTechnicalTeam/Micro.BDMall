using BDMall.Domain;
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
        List<ShopcartItem> GetShoppingCartItem();

        Task<SystemResult> AddtoCartAsync(CartItem cartItem);

        Task<SystemResult> UpdateCartItemAsync(Guid itemId, int qty);

        Task<SystemResult> RemoveFromCart(Guid itemId);

        Task<SystemResult> ClearShoppingCart();
    }

}
