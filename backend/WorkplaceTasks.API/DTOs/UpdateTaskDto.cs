using System.ComponentModel.DataAnnotations;
using WorkplaceTasks.API.Models;

namespace WorkplaceTasks.API.DTOs;

/// <summary>
/// DTO used to update an existing task.
/// All properties are optional to allow partial updates.
/// </summary>
public class UpdateTaskDto
{
    // Optional new title
    public string? Title { get; set; }

    // Optional new description
    public string? Description { get; set; }

    // Optional status update (Pending, InProgress, Done)
    public TaskItemStatus? Status { get; set; }

    // Optional reassignment (only allowed for Admin/Manager)
    public Guid? AssignedToId { get; set; }
}
