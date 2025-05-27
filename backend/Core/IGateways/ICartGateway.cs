using System;
using Core.Models;

namespace Core.IGateways;

public interface ICartGateway
{
    Cart? GetCartByUserId(Guid userId);
    Cart CreateCart(Guid userId);
    void AddOrUpdateItemsToCart(Guid cartId, IEnumerable<CartItem> items);
    void ClearCart(Guid cartId);
}
