using Microsoft.EntityFrameworkCore;
using WorkplaceTasks.API.Models;

namespace WorkplaceTasks.API.Data;

/// <summary>
/// Database context responsible for mapping entities to the database.
/// Configures relationships and constraints.
/// </summary>
public class AppDbContext : DbContext
{
    // Users table
    public DbSet<User> Users => Set<User>();

    // Tasks table
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Enforce unique email for users
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // One User (CreatedBy) -> Many Tasks
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.CreatedBy)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.CreatedById);

        // Optional AssignedTo relationship
        // Restrict delete to prevent removing users with assigned tasks
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.AssignedTo)
            .WithMany()
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
