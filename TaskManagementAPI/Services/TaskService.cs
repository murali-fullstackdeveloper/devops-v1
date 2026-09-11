using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories;

namespace TaskManagementAPI.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskResponseDto>> GetTasksForUserAsync(int userId)
    {
        var tasks = await _taskRepository.GetAllByUserIdAsync(userId);
        return tasks.Select(MapToDto);
    }

    public async Task<TaskResponseDto?> GetTaskByIdForUserAsync(int taskId, int userId)
    {
        var task = await _taskRepository.GetByIdAndUserIdAsync(taskId, userId);
        return task == null ? null : MapToDto(task);
    }

    public async Task<TaskResponseDto> CreateTaskForUserAsync(CreateTaskDto dto, int userId)
    {
        var task = new TaskItem
        {
            Title = dto.Title.Trim(),
            Description = dto.Description?.Trim() ?? string.Empty,
            Status = dto.Status,
            Priority = dto.Priority,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        var createdTask = await _taskRepository.AddAsync(task);
        return MapToDto(createdTask);
    }

    public async Task<TaskResponseDto?> UpdateTaskForUserAsync(int taskId, UpdateTaskDto dto, int userId)
    {
        var task = await _taskRepository.GetByIdAndUserIdAsync(taskId, userId);
        if (task == null)
        {
            return null;
        }

        task.Title = dto.Title.Trim();
        task.Description = dto.Description?.Trim() ?? string.Empty;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.UpdatedAt = DateTime.UtcNow;

        var updatedTask = await _taskRepository.UpdateAsync(task);
        return MapToDto(updatedTask);
    }

    public async Task<bool> DeleteTaskForUserAsync(int taskId, int userId)
    {
        return await _taskRepository.DeleteAsync(taskId, userId);
    }

    private static TaskResponseDto MapToDto(TaskItem task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            Priority = task.Priority.ToString(),
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            UserId = task.UserId
        };
    }
}
