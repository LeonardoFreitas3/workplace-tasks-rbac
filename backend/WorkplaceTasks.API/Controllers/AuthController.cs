using Microsoft.AspNetCore.Mvc;
using WorkplaceTasks.API.Data;
using WorkplaceTasks.API.DTOs;
using WorkplaceTasks.API.Services;
using BCrypt.Net;

/// <summary>
/// Responsible for authentication operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    /// Injects database context and JWT service.
    public AuthController(AppDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token if credentials are valid.
    /// </summary>
    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        // Find user by email
        var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

        // Validate user existence and password using BCrypt hash verification
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized();

        // Generate JWT token containing user claims (Id, Email, Role)
        var token = _jwtService.GenerateToken(user);

        // Return token to client
        return Ok(new { token });
    }
}
