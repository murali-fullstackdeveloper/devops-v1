namespace TaskManagementAPI.Models;

/// <summary>
/// Domain model representing a registered User.
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property for user's task collection
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
