using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkplaceTasks.API.Data;
using WorkplaceTasks.API.DTOs;
using WorkplaceTasks.API.Helpers;
using WorkplaceTasks.API.Models;

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

    // =========================
    // GET /tasks (com filtros + paginação)
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetTasks(
      [FromQuery] TaskItemStatus? status,
      [FromQuery] int page = 1,
      [FromQuery] int pageSize = 10)
    {
        var userId = UserClaimsHelper.GetUserId(User);
        var role = UserClaimsHelper.GetUserRole(User);

        var query = _context.Tasks.AsQueryable();

        // =========================
        // RBAC filtering
        // =========================
        if (role == "Member")
        {
            query = query.Where(t =>
                t.CreatedById == userId ||
                t.AssignedToId == userId
            );
        }

        // =========================
        // Status filter (ONLY once)
        // =========================
        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        // =========================
        // Pagination
        // =========================
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



    // =========================
    // POST /tasks
    // =========================
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
            Status = TaskItemStatus.Pending, // default
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedById = userId
        };

        // Only Admin or Manager can assign tasks
        if ((role == "Admin" || role == "Manager") && dto.AssignedToId.HasValue)
        {
            task.AssignedToId = dto.AssignedToId;
        }

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
    }


    // =========================
    // PUT /tasks/{id}
    // =========================
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskDto dto)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
            return NotFound();

        var userId = UserClaimsHelper.GetUserId(User);
        var role = UserClaimsHelper.GetUserRole(User);

        if (role == "Admin" || role == "Manager")
        {
            // Full access
        }
        else if (role == "Member")
        {
            if (task.CreatedById != userId)
            {
                // Only allow status update if assigned
                if (task.AssignedToId != userId || dto.Status == null)
                    return Forbid();

                // Only update status
                task.Status = dto.Status.Value;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        if (dto.Title != null)
            task.Title = dto.Title;

        if (dto.Description != null)
            task.Description = dto.Description;

        if (dto.Status.HasValue)
            task.Status = dto.Status.Value;

        if (dto.AssignedToId.HasValue)
            task.AssignedToId = dto.AssignedToId;

        await _context.SaveChangesAsync();

        return NoContent();
    }



    // =========================
    // DELETE /tasks/{id}
    // =========================
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
