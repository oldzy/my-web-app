using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;
using System.Collections.Generic;

namespace Core.UseCases;

public class ProductUseCases : IProductUseCases
{
    private readonly IProductGateway _productGateway;

    public ProductUseCases(IProductGateway productGateway)
    {
        _productGateway = productGateway ?? throw new ArgumentNullException(nameof(productGateway));
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _productGateway.GetAllProducts();
    }

    public void AddProduct(Product product)
    {
        if (product.Price <= 0)
        {
            throw new ArgumentException("Product price must be greater than zero.", nameof(product.Price));
        }

        if (product.Stock < 0)
        {
            throw new ArgumentException("Product stock cannot be negative.", nameof(product.Stock));
        }
        
        _productGateway.AddProduct(product);
    }
}
