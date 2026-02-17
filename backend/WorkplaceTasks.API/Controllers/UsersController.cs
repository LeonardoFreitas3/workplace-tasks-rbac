using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkplaceTasks.API.Data;
using WorkplaceTasks.API.DTOs;
using WorkplaceTasks.API.Helpers;
using WorkplaceTasks.API.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Base protection
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET: /api/users
    // Admin + Manager can list users
    // =========================
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetUsers()
    {
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

    // =========================
    // PUT: /api/users/{id}/role
    // Only Admin can change roles
    // =========================
    [HttpPut("{id}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRole(
        Guid id,
        [FromBody] UpdateUserRoleDto dto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        user.Role = dto.Role;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // =========================
    // POST: /api/users
    // Only Admin can create users
    // =========================
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // =========================
    // DELETE: /api/users/{id}
    // Only Admin can delete users
    // =========================
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        var currentUserId = UserClaimsHelper.GetUserId(User);

        // Prevent deleting yourself
        if (user.Id == currentUserId)
            return BadRequest("You cannot delete your own account.");

        // Prevent deleting the last Admin
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
