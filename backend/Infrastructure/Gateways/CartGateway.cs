using Core.IGateways;
using Core.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Gateways;

public class CartGateway : ICartGateway
{
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;

    public CartGateway(IProductRepository productRepository, ICartRepository cartRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
    }

    public void AddOrUpdateItemsToCart(Guid cartId, IEnumerable<CartItem> items)
    {
        var itemsDb = items.Select(i => new Models.CartItem
        {
            Quantity = i.Quantity,
            ProductId = i.Product.Id,
            Price = i.UnitPrice
        });

        var itemsToAdd = new List<Models.CartItem>();
        var itemsToUpdate = new List<Models.CartItem>();

        foreach (var item in itemsDb)
        {
            var cartItem = _cartRepository.GetCartItemById(cartId, item.ProductId);

            if (cartItem == null)
            {
                itemsToAdd.Add(item);
            }
            else
            {
                cartItem.Quantity = item.Quantity;
                cartItem.Price = item.Price;

                itemsToUpdate.Add(cartItem);
            }
        }

        if (itemsToAdd.Count != 0)
        {
            _cartRepository.AddItemsToCart(cartId, itemsToAdd);
        }
        if (itemsToUpdate.Count != 0)
        {
            _cartRepository.UpdateItemsInCart(cartId, itemsToUpdate);
        }
    }

    public void ClearCart(Guid cartId)
    {
        _cartRepository.ClearCart(cartId);
    }

    public Cart CreateCart(Guid userId)
    {
        var cartDb = _cartRepository.CreateCart(userId);
        var cart = new Cart
        {
            Id = cartDb.Id,
            CartItems = new List<CartItem>()
        };

        return cart;
    }

    public Cart? GetCartByUserId(Guid userId)
    {
        Cart? res = null;
        var cartDb = _cartRepository.GetCartByUserId(userId);

        if (cartDb != null)
        {
            res = new Cart
            {
                Id = cartDb.Id,
                CartItems = cartDb.CartItems.Select(i =>
                {
                    return new CartItem
                    {
                        Product = new Product
                        {
                            Id = i.ProductId,
                        },
                        Quantity = i.Quantity,
                        UnitPrice = i.Price
                    };
                }).ToList()
            };

            foreach (var item in res.CartItems)
            {
                var product = _productRepository.GetProductById(item.Product.Id);
                if (product != null)
                {
                    item.Product = new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Stock = product.Stock,
                        Image = !string.IsNullOrEmpty(product.ImageUrl) ? Base64Utils.GetBase64Image(product.ImageUrl) : product.ImageUrl
                    };
                }
            }
        }
        
        return res;
    }
}
