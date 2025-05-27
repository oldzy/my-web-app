using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions
{
    public interface ICartRepository
    {
        Cart? GetCartByUserId(Guid userId);
        Cart CreateCart(Guid userId);
        CartItem? GetCartItemById(Guid cartId, Guid productId);
        void AddItemsToCart(Guid cartId, IEnumerable<CartItem> items);
        void UpdateItemsInCart(Guid cartId, IEnumerable<CartItem> items);
        void ClearCart(Guid cartId);
    }
}
