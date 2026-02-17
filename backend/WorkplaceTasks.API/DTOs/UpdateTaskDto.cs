using System.ComponentModel.DataAnnotations;
using WorkplaceTasks.API.Models;

namespace WorkplaceTasks.API.DTOs;

public class UpdateTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskItemStatus? Status { get; set; }
    public Guid? AssignedToId { get; set; }
}

