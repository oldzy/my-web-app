using System;
using Core.Models;

namespace Core.IGateways;

public interface IUserGateway
{
    string? GetUserPasswordHash(string username);
    User? GetUserByUsername(string username);
    void AddUser(string username, string passwordHash);
    IEnumerable<User> GetAllUsers();
}
