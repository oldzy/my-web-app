using System.Data;
using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Infrastructure.Repositories;

public class ProductRepository(IConfiguration configuration) : IProductRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentNullException(nameof(configuration), "Database connection string 'DefaultConnection' not found.");

    private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

    public IEnumerable<Product> GetAllProducts()
    {
        const string sql = "SELECT Id, Name, Description, Price, Stock, ImageUrl, CreatedAt, UpdatedAt FROM Product;";
        using var connection = CreateConnection();
        return connection.Query<Product>(sql);
    }

    public void AddProduct(Product product)
    {
        const string sql = "INSERT INTO Product (Name, Description, Price, Stock, ImageUrl) VALUES (@Name, @Description, @Price, @Stock, @ImageUrl);";
        using var connection = CreateConnection();
        connection.Execute(sql, product);
    }
}
