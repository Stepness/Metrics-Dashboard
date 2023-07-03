namespace MetricsMonitoringServer.Services;

public class FakeRepository: IRepository
{
    private List<User> _users = new List<User>
    {
        new User 
        {
            Id = 1, Username = "peter", Password = "peter123", Role      = "Admin"
        },
        new User
        {
            Id = 2, Username = "joydip", Password = "joydip123", Role = "Viewer"
        },
        new User
        {
            Id = 3, Username = "james", Password = "james123"
        }
    };
    public async Task<User?> Authenticate(string username, string password)
    {
        return await Task.FromResult(_users.SingleOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && x.Password == password)) ?? null;
    }
    
    public async Task<List<string>> GetUserNames()
    {
        var users = _users.Select(user => user.Username).ToList();
        
        return await Task.FromResult(users);
    }
}