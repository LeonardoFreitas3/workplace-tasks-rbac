using Microsoft.EntityFrameworkCore;
using WorkplaceTasks.API.Models;

namespace WorkplaceTasks.API.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.CreatedBy)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.CreatedById);

        modelBuilder.Entity<TaskItem>()
        .HasOne(t => t.AssignedTo)
        .WithMany()
        .HasForeignKey(t => t.AssignedToId)
        .OnDelete(DeleteBehavior.Restrict);

    }
}
