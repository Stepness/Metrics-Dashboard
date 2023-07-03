namespace MetricsMonitoringServer.Services;

public interface IRepository
{
    Task<User?> Authenticate(string username, string password);
    Task<List<string>> GetUserNames();
}


public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}