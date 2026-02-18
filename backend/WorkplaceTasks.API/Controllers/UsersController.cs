using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkplaceTasks.API.Data;
using WorkplaceTasks.API.DTOs;
using WorkplaceTasks.API.Helpers;
using WorkplaceTasks.API.Models;

/// <summary>
/// Manages user operations and role administration.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns list of users (Id, Email, Role).
    /// Accessible by Admin and Manager.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetUsers()
    {
        // Return minimal user data
        var users = await _context.Users
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.Role
            })
            .ToListAsync();

        return Ok(users);
    }

    /// <summary>
    /// Updates a user's role.
    /// Only Admin can change roles.
    /// </summary>
    [HttpPut("{id}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRole(
        Guid id,
        [FromBody] UpdateUserRoleDto dto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        // Update role according to business rules
        user.Role = dto.Role;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Creates a new user.
    /// Only Admin can create users.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        // Ensure email uniqueness
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,

            // Password stored securely using BCrypt hashing
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),

            Role = dto.Role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Deletes a user.
    /// Only Admin can delete users.
    /// Includes safety checks.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        var currentUserId = UserClaimsHelper.GetUserId(User);

        // Prevent deleting your own account
        if (user.Id == currentUserId)
            return BadRequest("You cannot delete your own account.");

        // Prevent deleting the last Admin in the system
        if (user.Role == UserRole.Admin)
        {
            var adminCount = await _context.Users
                .CountAsync(u => u.Role == UserRole.Admin);

            if (adminCount <= 1)
                return BadRequest("Cannot delete the last admin.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
