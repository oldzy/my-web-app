using Core.Models;

namespace Core.IGateways;

public interface IProductGateway
{
    IEnumerable<Product> GetAllProducts();
    void AddProduct(Product product);
}
