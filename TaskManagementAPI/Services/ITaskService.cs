using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskResponseDto>> GetTasksForUserAsync(int userId);
    Task<TaskResponseDto?> GetTaskByIdForUserAsync(int taskId, int userId);
    Task<TaskResponseDto> CreateTaskForUserAsync(CreateTaskDto dto, int userId);
    Task<TaskResponseDto?> UpdateTaskForUserAsync(int taskId, UpdateTaskDto dto, int userId);
    Task<bool> DeleteTaskForUserAsync(int taskId, int userId);
}
