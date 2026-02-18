namespace WorkplaceTasks.API.Models
{
    /// <summary>
    /// Represents a system user.
    /// Contains authentication and authorization information.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; } /// Unique identifier (UUID).
        public string Email { get; set; } = string.Empty; /// Unique email used for authentication.

        public string PasswordHash { get; set; } = string.Empty; /// Hashed password (never store plain text passwords).

        public UserRole Role { get; set; } /// Role used for RBAC (Admin, Manager, Member).

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>(); /// Tasks created by this user (owner relationship).
    }
}
