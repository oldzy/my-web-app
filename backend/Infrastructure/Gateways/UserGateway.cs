using Core.IGateways;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Gateways;

public class UserGateway : IUserGateway
{
    private readonly IUserRepository _userRepository;

    public UserGateway(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void AddUser(string username, string passwordHash)
    {
        var user = new User
        {
            Username = username,
            PasswordHash = passwordHash,
            IsAdmin = false
        };

        _userRepository.AddUser(user);
    }

    public IEnumerable<Core.Models.User> GetAllUsers()
    {
        var users = _userRepository.GetAllUsers();
        return users.Select(user => new Core.Models.User
        {
            Id = user.Id,
            Username = user.Username,
            IsAdmin = user.IsAdmin
        });
    }

    public string? GetUserPasswordHash(string username) 
    {
        var user = _userRepository.GetUserByUsername(username);
        return user?.PasswordHash;
    }

    public Core.Models.User? GetUserByUsername(string username)
    {
        var infraUser = _userRepository.GetUserByUsername(username);
        if (infraUser == null) return null;
        return new Core.Models.User
        {
            Id = infraUser.Id,
            Username = infraUser.Username,
            IsAdmin = infraUser.IsAdmin
        };
    }
}
