using System;

namespace Core.Models;

public class User
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public bool IsAdmin { get; set; }
}
