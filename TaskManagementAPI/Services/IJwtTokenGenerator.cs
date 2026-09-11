using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services;

public interface IJwtTokenGenerator
{
    (string token, DateTime expiresAt) GenerateToken(User user);
}
