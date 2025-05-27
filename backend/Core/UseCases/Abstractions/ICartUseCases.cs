using Core.Models;

namespace Core.UseCases.Abstractions;

public interface ICartUseCases
{
    Cart GetCartByUserId(Guid userId);
    void AddOrUpdateItemsToCart(Guid cartId, IEnumerable<CartItem> items);
    void ClearCart(Guid userId);
}
