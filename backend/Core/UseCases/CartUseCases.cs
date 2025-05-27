using System;
using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

public class CartUseCases : ICartUseCases
{
    private readonly ICartGateway _cartGateway;
    private readonly IProductGateway _productGateway;

    public CartUseCases(ICartGateway cartGateway, IProductGateway productGateway)
    {
        _cartGateway = cartGateway ?? throw new ArgumentNullException(nameof(cartGateway));
        _productGateway = productGateway ?? throw new ArgumentNullException(nameof(productGateway));
    }

    public void AddOrUpdateItemsToCart(Guid cartId, IEnumerable<CartItem> items)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));
        }

        if (items == null || !items.Any())
        {
            throw new ArgumentException("Items cannot be null or empty.", nameof(items));
        }

        foreach (var item in items)
        {
            if (item.Quantity <= 0)
            {
                throw new ArgumentException("Item quantity must be greater than zero.", nameof(item.Quantity));
            }

            var product = _productGateway.GetProductById(item.Product.Id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {item.Product.Id} not found.");
            }
            
            if (item.Quantity > product.Stock)
            {
                throw new ArgumentException($"Insufficient stock for product {product.Name}. Available: {product.Stock}, Requested: {item.Quantity}.");
            }
        }

        _cartGateway.AddOrUpdateItemsToCart(cartId, items);        
    }

    public void ClearCart(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Cart ID cannot be empty.", nameof(cartId));
        }

        var cart = _cartGateway.GetCartByUserId(cartId);
        if (cart == null)
        {
            throw new KeyNotFoundException("Cart not found.");
        }

        _cartGateway.ClearCart(cartId);
    }

    public Cart GetCartByUserId(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        var cart = _cartGateway.GetCartByUserId(userId);

        if (cart == null)
        {
            cart = _cartGateway.CreateCart(userId);
        }

        return cart;
    }
}
