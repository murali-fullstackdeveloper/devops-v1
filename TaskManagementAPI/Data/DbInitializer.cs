using TaskManagementAPI.Helpers;
using TaskManagementAPI.Models;
using TaskStatus = TaskManagementAPI.Models.TaskStatus;

namespace TaskManagementAPI.Data;

/// <summary>
/// Database Initializer for applying pending migrations and seeding initial sample data.
/// </summary>
public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
    {
        // Seed default user if no users exist
        if (!context.Users.Any())
        {
            var seedUser = new User
            {
                Name = "Demo User",
                Email = "user@example.com",
                PasswordHash = passwordHasher.HashPassword("Password123!"),
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(seedUser);
            await context.SaveChangesAsync();

            // Seed sample tasks for demo user
            var seedTasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Title = "Setup PostgreSQL Database",
                    Description = "Install PostgreSQL and run initial Entity Framework Core migrations.",
                    Status = TaskStatus.Completed,
                    Priority = TaskPriority.High,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UserId = seedUser.Id
                },
                new TaskItem
                {
                    Title = "Implement JWT Authentication",
                    Description = "Secure API endpoints using Bearer token authentication.",
                    Status = TaskStatus.InProgress,
                    Priority = TaskPriority.High,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UserId = seedUser.Id
                },
                new TaskItem
                {
                    Title = "Write Unit Tests",
                    Description = "Add xUnit tests for Auth and Task services.",
                    Status = TaskStatus.Pending,
                    Priority = TaskPriority.Medium,
                    CreatedAt = DateTime.UtcNow,
                    UserId = seedUser.Id
                }
            };

            context.Tasks.AddRange(seedTasks);
            await context.SaveChangesAsync();
        }
    }
}
