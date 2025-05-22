using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Infrastructure.Repositories;

public class UserRepository(IConfiguration configuration) : IUserRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentNullException(nameof(configuration), "Database connection string 'DefaultConnection' not found.");

    private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

    public User? GetUserByUsername(string username)
    {
        const string sql = "SELECT Id, Username, PasswordHash, IsAdmin, CreatedAt, UpdatedAt FROM Users WHERE Username = @Username;";
        using var connection = CreateConnection();
        return connection.QuerySingleOrDefault<User?>(sql, new { Username = username });
    }

    public void AddUser(User user)
    {
        const string sql = "INSERT INTO Users (Username, PasswordHash, IsAdmin) VALUES (@Username, @PasswordHash, @IsAdmin);";
        using var connection = CreateConnection();
        connection.Execute(sql, new
        {
            user.Username,
            user.PasswordHash,
            user.IsAdmin
        });
    }

    public IEnumerable<User> GetAllUsers()
    {
        const string sql = "SELECT Id, Username, PasswordHash, IsAdmin, CreatedAt, UpdatedAt FROM Users;";
        using var connection = CreateConnection();
        return connection.Query<User>(sql);
    }
}
