using WorkplaceTasks.API.Models;

namespace WorkplaceTasks.API.DTOs
{
    /// <summary>
    /// DTO used to update a user's role.
    /// Only accessible by Admin users.
    /// </summary>
    public class UpdateUserRoleDto
    {
        // New role to assign (Admin, Manager or Member)
        public UserRole Role { get; set; }
    }
}
