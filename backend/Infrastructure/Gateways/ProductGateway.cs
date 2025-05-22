using Core.IGateways;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Gateways;

public class ProductGateway : IProductGateway
{
    private readonly IProductRepository _productRepository;

    public ProductGateway(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    private string? GetBase64ImageAsync(string? imageUrl)
    {
        using var httpClient = new HttpClient();

        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return null;
        }

        try
        {
            var response = httpClient.GetAsync(imageUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                var imageBytes = response.Content.ReadAsByteArrayAsync().Result;
                return Convert.ToBase64String(imageBytes);
            }            
            return null;
        }
        catch (Exception)
        {
            return null;
        }
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
                imageBase64 = GetBase64ImageAsync(p.ImageUrl);
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
}
