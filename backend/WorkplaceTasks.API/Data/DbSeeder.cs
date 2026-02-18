using WorkplaceTasks.API.Models;
using BCrypt.Net;

namespace WorkplaceTasks.API.Data;

/// <summary>
/// Seeds the database with initial test users.
/// </summary>
public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Prevent duplicate seeding
        if (context.Users.Any())
            return;

        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Role = UserRole.Admin
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "manager@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Role = UserRole.Manager
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "member@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Role = UserRole.Member
            }
        };

        context.Users.AddRange(users);
        context.SaveChanges();
    }
}
