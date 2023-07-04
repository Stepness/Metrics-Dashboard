using MetricsMonitoringServer.Models;
using MetricsMonitoringServer.Settings;

namespace MetricsMonitoringServer.Services;

public class FakeRepository: IRepository
{
    private readonly List<UserEntity> _users = new()
    {
        new UserEntity
        {
            Id = Guid.NewGuid().ToString(), Username = "peter", Password = "peter123", Role      = "Admin"
        },
        new UserEntity
        {
            Id = Guid.NewGuid().ToString(), Username = "james", Password = "james123"
        }
    };
    public async Task<UserEntity?> Authenticate(string username, string password)
    {
        return await Task.FromResult(_users.SingleOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && x.Password == password)) ?? null;
    }
    
    public async Task<List<UserEntity>> GetAllUsers()
    {
        var users = _users.ToList();
        
        return await Task.FromResult(users);
    }

    public Task<AddUserResult> AddUserAsync(UserEntity user)
    {
        _users.Add(user);
        return Task.FromResult(new AddUserResult
        {
            Result = AddUserResultType.Success,
            User = user
        });
    }

    public Task<bool> UpdateUserRoleToViewer(string username)
    {
        var user = _users.First(x => x.Username == username);
        
        if (user.Role == Roles.Admin)
            return Task.FromResult(false);
        
        user.Role = Roles.Viewer;
        return Task.FromResult(true);
    }
}