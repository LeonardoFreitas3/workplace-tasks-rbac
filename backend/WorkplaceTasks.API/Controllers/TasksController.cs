using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkplaceTasks.API.Data;
using WorkplaceTasks.API.DTOs;
using WorkplaceTasks.API.Helpers;
using WorkplaceTasks.API.Models;

/// <summary>
/// Manages task operations with RBAC rules.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns tasks with optional status filter and pagination.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetTasks(
      [FromQuery] TaskItemStatus? status,
      [FromQuery] int page = 1,
      [FromQuery] int pageSize = 10)
    {
        // Extract user info from JWT
        var userId = UserClaimsHelper.GetUserId(User);
        var role = UserClaimsHelper.GetUserRole(User);

        var query = _context.Tasks.AsQueryable();

        // Members can only see tasks they created or were assigned
        if (role == "Member")
        {
            query = query.Where(t =>
                t.CreatedById == userId ||
                t.AssignedToId == userId
            );
        }

        // Apply optional status filter
        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        // Server-side pagination
        var totalCount = await query.CountAsync();

        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new
            {
                t.Id,
                t.Title,
                t.Description,
                t.Status,
                t.CreatedAt,
                t.UpdatedAt,
                t.CreatedById,
                t.AssignedToId,
                AssignedToEmail = t.AssignedTo != null ? t.AssignedTo.Email : null
            })
            .ToListAsync();

        return Ok(new
        {
            totalCount,
            page,
            pageSize,
            data = tasks
        });
    }

    /// <summary>
    /// Creates a new task.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
    {
        var userId = UserClaimsHelper.GetUserId(User);
        var role = UserClaimsHelper.GetUserRole(User);

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Status = TaskItemStatus.Pending, // default status
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedById = userId
        };

        // Assignment allowed only for Admin and Manager
        if ((role == "Admin" || role == "Manager") && dto.AssignedToId.HasValue)
        {
            task.AssignedToId = dto.AssignedToId;
        }

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
    }

    /// <summary>
    /// Updates a task according to RBAC rules.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskDto dto)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
            return NotFound();

        var userId = UserClaimsHelper.GetUserId(User);
        var role = UserClaimsHelper.GetUserRole(User);

        // Admin and Manager have full edit access
        if (role == "Admin" || role == "Manager")
        {
            // Full access
        }
        else if (role == "Member")
        {
            // If not creator, only allow status update if assigned
            if (task.CreatedById != userId)
            {
                if (task.AssignedToId != userId || dto.Status == null)
                    return Forbid();

                task.Status = dto.Status.Value;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        // Apply updates if provided
        if (dto.Title != null)
            task.Title = dto.Title;

        if (dto.Description != null)
            task.Description = dto.Description;

        if (dto.Status.HasValue)
            task.Status = dto.Status.Value;

        if (dto.AssignedToId.HasValue)
            task.AssignedToId = dto.AssignedToId;

        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Deletes a task.
    /// Admin can delete any task.
    /// Others can delete only their own tasks.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
            return NotFound();

        var userId = UserClaimsHelper.GetUserId(User);
        var role = UserClaimsHelper.GetUserRole(User);

        if (role != "Admin" && task.CreatedById != userId)
            return Forbid();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
