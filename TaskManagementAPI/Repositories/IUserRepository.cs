using TaskManagementAPI.Models;

namespace TaskManagementAPI.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task<User> AddAsync(User user);
}
