using System.ComponentModel.DataAnnotations;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.DTOs;

/// <summary>
/// Payload data transfer object for creating a new Task.
/// </summary>
public class CreateTaskDto
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string Description { get; set; } = string.Empty;

    public TaskManagementAPI.Models.TaskStatus Status { get; set; } = TaskManagementAPI.Models.TaskStatus.Pending;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
}
