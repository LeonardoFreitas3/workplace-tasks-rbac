using WorkplaceTasks.API.Models;

namespace WorkplaceTasks.API.DTOs;

public class CreateUserDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}
