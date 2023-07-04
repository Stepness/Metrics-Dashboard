using MetricsMonitoringServer.Models;

namespace MetricsMonitoringServer.Services;

public interface IRepository
{
    Task<UserEntity?> Authenticate(string username, string password);
    Task<List<UserEntity>> GetAllUsers();
    Task<AddUserResult> AddUserAsync(UserEntity user);
}
