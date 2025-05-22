using Core.Models;
using System.Collections.Generic;

namespace Core.UseCases.Abstractions;

public interface IProductUseCases
{
    IEnumerable<Product> GetAllProducts();
    void AddProduct(Product product);
}
