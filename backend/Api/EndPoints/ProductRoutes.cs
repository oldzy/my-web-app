using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class ProductRoutes
{
    public static WebApplication AddProductRoutes(this WebApplication app)
    {
        var group = app.MapGroup("api/products")
            .RequireAuthorization()
            .WithTags("Products");

        group.MapGet("", (IProductUseCases productUseCases) =>
        {
            var products = productUseCases.GetAllProducts();
            return Results.Ok(products);
        })
        .WithName("GetAllProducts")
        .Produces<IEnumerable<Product>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("", (Product product, IProductUseCases productUseCases) =>
        {
            productUseCases.AddProduct(product);
            return Results.Created();
        })
        .WithName("AddProduct")
        .RequireAuthorization("AdminOnly")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }
}
