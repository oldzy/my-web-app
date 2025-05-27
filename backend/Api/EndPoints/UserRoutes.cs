using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class UserRoutes
{
    private static (Cart cart, Guid userId) GetCartByUserId(ICartUseCases cartUseCases, HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new ArgumentException("Invalid user identifier format in token.");
        }

        return (cartUseCases.GetCartByUserId(userId), userId);
    }
    
    public static WebApplication AddUserRoutes(this WebApplication app)
    {
        var group = app.MapGroup("api/users")
            .RequireAuthorization()
            .WithTags("Users");

        group.MapGet("", (IUserUseCases userUseCases) =>
        {
            var users = userUseCases.GetAllUsers();
            return Results.Ok(users);
        })
        .WithName("GetAllUsers")
        .Produces<IEnumerable<User>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("/auth", ([FromBody] AuthenticationRequest request, IUserUseCases userUseCases, IConfiguration configuration) =>
        {
            var user = userUseCases.AuthenticateAndGetUser(request);

            if (user != null)
            {
                var issuer = configuration["Jwt:Issuer"];
                var audience = configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
                var expireTime = configuration["Jwt:ExpireTimeInMinutes"];
                var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(expireTime ?? "5"));

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                };

                if (user.IsAdmin)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = expiration,
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return Results.Ok(new { token = jwtToken });
            }
            else
            {
                return Results.Unauthorized();
            }
        })
        .AllowAnonymous()
        .WithName("Auth")
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);


        group.MapPost("/register", ([FromBody] RegisterRequest request, IUserUseCases userUseCases) =>
        {
            userUseCases.Register(request);
            return Results.Ok(new { message = "User registered successfully" });
        })
        .AllowAnonymous()
        .WithName("Register")
        .Produces<object>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("me/cart", (ICartUseCases cartUseCases, HttpContext httpContext) =>
        {
            var cart = GetCartByUserId(cartUseCases, httpContext).cart;
            return Results.Ok(cart);
        })
        .WithName("GetMyCart")
        .Produces<Cart>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("me/cart/items", ([FromBody] IEnumerable<Models.CartItem> items, ICartUseCases cartUseCases, HttpContext httpContext) =>
        {
            var res = GetCartByUserId(cartUseCases, httpContext);
            var cart = res.cart;
            var userId = res.userId;

            cartUseCases.AddOrUpdateItemsToCart(cart.Id, items.Select(i => new CartItem
            {
                Product = new Product { Id = i.ProductId },
                Quantity = i.Quantity,
                UnitPrice = i.Price
            }));

            cart = cartUseCases.GetCartByUserId(userId);

            return Results.Ok(cart);
        })
        .WithName("AddOrUpdateCartItems")
        .Produces<Cart>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);


        group.MapDelete("me/cart/items", (ICartUseCases cartUseCases, HttpContext httpContext) =>
        {
            var cart = GetCartByUserId(cartUseCases, httpContext).cart;

            cartUseCases.ClearCart(cart.Id);

            return Results.Ok();
        })
        .WithName("ClearCart")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }
}
