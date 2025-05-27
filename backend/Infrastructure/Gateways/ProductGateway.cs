using Core.IGateways;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Gateways;

public class ProductGateway : IProductGateway
{
    private readonly IProductRepository _productRepository;

    public ProductGateway(IProductRepository productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public IEnumerable<Core.Models.Product> GetAllProducts()
    {
        var infraProducts = _productRepository.GetAllProducts();
        var coreProducts = new List<Core.Models.Product>();

        foreach (var p in infraProducts)
        {
            string? imageBase64 = null;
            if (!string.IsNullOrWhiteSpace(p.ImageUrl))
            {
                imageBase64 = Base64Utils.GetBase64Image(p.ImageUrl);
            }
            coreProducts.Add(new Core.Models.Product
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                Image = imageBase64
            });
        }
        return coreProducts;
    }

    public void AddProduct(Core.Models.Product product)
    {
        var infraProduct = new Product
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.Image
        };
        _productRepository.AddProduct(infraProduct);
    }

    public Core.Models.Product? GetProductById(Guid productId)
    {
        var infraProduct = _productRepository.GetProductById(productId);
        if (infraProduct == null)
        {
            return null;
        }

        string? imageBase64 = null;
        if (!string.IsNullOrWhiteSpace(infraProduct.ImageUrl))
        {
            imageBase64 = Base64Utils.GetBase64Image(infraProduct.ImageUrl);
        }

        return new Core.Models.Product
        {
            Id = infraProduct.Id,
            Name = infraProduct.Name,
            Description = infraProduct.Description,
            Price = infraProduct.Price,
            Stock = infraProduct.Stock,
            Image = imageBase64
        };
    }
}
