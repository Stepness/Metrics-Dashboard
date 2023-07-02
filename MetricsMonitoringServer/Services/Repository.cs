namespace MetricsMonitoringServer.Services;

public class Repository: IRepository
{
    private List<User> _users = new List<User>
    {
        new User 
        {
            Id = 1, Username = "peter", Password = "peter123"            
        },
        new User
        {
            Id = 2, Username = "joydip", Password = "joydip123"
        },
        new User
        {
            Id = 3, Username = "james", Password = "james123"
        }
    };
    public async Task<bool> Authenticate(string username, string password)
    {
        if(await Task.FromResult(_users.SingleOrDefault(x => x.Username == username && x.Password == password)) != null)
        {
            return true;
        }
        return false;
    }
    
    public async Task<List<string>> GetUserNames()
    {
        var users = _users.Select(user => user.Username).ToList();
        
        return await Task.FromResult(users);
    }
}