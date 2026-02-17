using System.ComponentModel.DataAnnotations;
using WorkplaceTasks.API.Models;


namespace WorkplaceTasks.API.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? AssignedToId { get; set; }
}
