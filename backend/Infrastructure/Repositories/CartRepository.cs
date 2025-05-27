using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly string _connectionString;

        public CartRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentNullException(nameof(configuration), "Database connection string 'DefaultConnection' not found.");
        }

        private MySqlConnection GetConnection() => new MySqlConnection(_connectionString);

        public Cart? GetCartByUserId(Guid userId)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM Cart WHERE UserId = @UserId";
            var cart = connection.QuerySingleOrDefault<Cart>(sql, new { UserId = userId });

            if (cart != null)
            {
                var cartItemsSql = "SELECT * FROM CartItem WHERE CartId = @CartId";
                var cartItems = connection.Query<CartItem>(cartItemsSql, new { CartId = cart.Id });
                cart.CartItems = [.. cartItems];
            }
            return cart;
        }

        public Cart CreateCart(Guid userId)
        {
            using var connection = GetConnection();

            var sql = "INSERT INTO Cart (UserId) VALUES (@UserId);";

            connection.Execute(sql, new { UserId = userId });

            return GetCartByUserId(userId) ?? throw new InvalidOperationException("Failed to create cart.");
        }

        public void AddItemsToCart(Guid cartId, IEnumerable<CartItem> items)
        {
            using var connection = GetConnection();

            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                foreach (var item in items)
                {
                    var cartItem = new CartItem
                    {
                        CartId = cartId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    var sql = "INSERT INTO CartItem (CartId, ProductId, Quantity, Price) VALUES (@CartId, @ProductId, @Quantity, @Price)";
                    connection.Execute(sql, cartItem, transaction);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void ClearCart(Guid cartId)
        {
            using var connection = GetConnection();
            var sql = "DELETE FROM CartItem WHERE CartId = @CartId";
            connection.Execute(sql, new { CartId = cartId });
        }

        public void UpdateItemsInCart(Guid cartId, IEnumerable<CartItem> items)
        {
            using var connection = GetConnection();

            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                foreach (var item in items)
                {
                    var cartItem = new CartItem
                    {
                        CartId = cartId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    
                    var sql = "UPDATE CartItem SET Quantity = @Quantity, Price = @Price WHERE CartId = @CartId AND ProductId = @ProductId";
                    connection.Execute(sql, cartItem, transaction);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public CartItem? GetCartItemById(Guid cartId, Guid productId)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM CartItem WHERE CartId = @CartId AND ProductId = @ProductId";
            return connection.QuerySingleOrDefault<CartItem>(sql, new { CartId = cartId, ProductId = productId });
        }
    }
}
