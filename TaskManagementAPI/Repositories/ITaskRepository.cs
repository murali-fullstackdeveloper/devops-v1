using TaskManagementAPI.Models;

namespace TaskManagementAPI.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllByUserIdAsync(int userId);
    Task<TaskItem?> GetByIdAndUserIdAsync(int id, int userId);
    Task<TaskItem> AddAsync(TaskItem task);
    Task<TaskItem> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(int id, int userId);
}
