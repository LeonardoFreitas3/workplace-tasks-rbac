using System.ComponentModel.DataAnnotations;
using WorkplaceTasks.API.Models;

namespace WorkplaceTasks.API.DTOs;

/// <summary>
/// DTO used to create a new task.
/// Separates API input model from the database entity (TaskItem).
/// </summary>
public class CreateTaskDto
{
    // Task title (required field from client)
    public string Title { get; set; } = string.Empty;

    // Task description
    public string Description { get; set; } = string.Empty;

    // Optional assignment (only Admin/Manager can assign)
    public Guid? AssignedToId { get; set; }
}
