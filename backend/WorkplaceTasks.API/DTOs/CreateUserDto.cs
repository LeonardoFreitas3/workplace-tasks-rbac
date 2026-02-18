using WorkplaceTasks.API.Models;

namespace WorkplaceTasks.API.DTOs;

/// <summary>
/// DTO used to create a new user.
/// Prevents exposing the full User entity (e.g., PasswordHash).
/// </summary>
public class CreateUserDto
{
    // User email (must be unique in the system)
    public string Email { get; set; } = string.Empty;

    // Plain password received from client (will be hashed before saving)
    public string Password { get; set; } = string.Empty;

    // Role assigned to the new user (Admin, Manager, Member)
    public UserRole Role { get; set; }
}
