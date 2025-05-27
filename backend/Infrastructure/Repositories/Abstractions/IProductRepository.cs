using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface IProductRepository
{
    IEnumerable<Product> GetAllProducts();
    Product? GetProductById(Guid productId);
    void AddProduct(Product product);
    int GetTotalQuantityOfProductInAllCarts(Guid productId);
}
