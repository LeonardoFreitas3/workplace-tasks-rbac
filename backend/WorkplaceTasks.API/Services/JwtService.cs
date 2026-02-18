using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WorkplaceTasks.API.Models;

namespace WorkplaceTasks.API.Services;

/// <summary>
/// Responsible for generating JWT tokens for authenticated users.
/// </summary>
public class JwtService
{
    private readonly IConfiguration _config;

    /// Injects configuration to access JWT settings (SecretKey, Issuer, Audience, Expiry).
    public JwtService(IConfiguration config)
    {
        _config = config;
    }

  
    /// Generates a signed JWT token containing user identity and role claims.
    public string GenerateToken(User user)
    {
        // Claims included inside the token
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        // Secret key used to sign the token
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create JWT token
        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_config["JwtSettings:ExpiryMinutes"]!)
            ),
            signingCredentials: creds
        );

        // Serialize token to string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
