using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface IProductRepository
{
    IEnumerable<Product> GetAllProducts();
    void AddProduct(Product product);
}
