using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;

namespace YourLocalShop.Models;

public class UsersRepository 
{
    private readonly ConcurrentDictionary<string, User> _users =
        new(StringComparer.OrdinalIgnoreCase);

    public PasswordHasher<User> Hasher { get; } = new();

    public UsersRepository()
    {
        var admin = new Employee
        {
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@shop.com",
            EmployeeRole = "Admin"
        };
        admin.PasswordHash = Hasher.HashPassword(admin, "Admin1234!");
        Add(admin);
    }

    public User? FindByEmail(string? email)
    {
        return string.IsNullOrWhiteSpace(email) ? null : _users.GetValueOrDefault(email);
    }

    public void Add(User user) => _users[user.Email] = user;

    public void Update(User user) => _users[user.Email] = user;
}