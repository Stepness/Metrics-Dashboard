namespace MetricsMonitoringServer.Services;

public interface IRepository
{
    Task<bool> Authenticate(string username, string password);
    Task<List<string>> GetUserNames();
}


public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}