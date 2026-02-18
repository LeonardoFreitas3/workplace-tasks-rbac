namespace WorkplaceTasks.API.DTOs;

/// <summary>
/// DTO used for user authentication.
/// Contains the credentials required to generate a JWT token.
/// </summary>
public class LoginDto
{
    // User email used for authentication
    public string Email { get; set; } = string.Empty;

    // Plain password sent by the client (validated against hashed password in database)
    public string Password { get; set; } = string.Empty;
}
