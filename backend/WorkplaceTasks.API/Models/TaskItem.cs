namespace WorkplaceTasks.API.Models
{
    /// <summary>
    /// Represents a task within the system.
    /// Contains ownership, assignment and status information.
    /// </summary>
    public class TaskItem
    {

       
        public Guid Id { get; set; }  /// Unique identifier of the task (UUID).

        public string Title { get; set; } = string.Empty; /// Short title describing the task.

        public string Description { get; set; } = string.Empty; /// Detailed description of the task.

        public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending; /// Current status of the task (Pending, InProgress, Done).

        /// Optional user assigned to execute the task.
        public Guid? AssignedToId { get; set; } 
        public User? AssignedTo { get; set; }

        public DateTime CreatedAt { get; set; } /// Timestamp when the task was created.

        public DateTime UpdatedAt { get; set; } /// Timestamp when the task was last updated.

        /// User who created the task (owner).
        public Guid CreatedById { get; set; }
        public User? CreatedBy { get; set; }
    }
}
