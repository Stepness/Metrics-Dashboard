using System.ComponentModel.DataAnnotations;

namespace MetricsMonitoringServer.Models;

public class SignDto
{
    [Required] public string Username { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;
}