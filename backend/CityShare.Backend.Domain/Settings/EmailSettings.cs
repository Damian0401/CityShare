namespace CityShare.Backend.Domain.Settings;

public class EmailSettings
{
    public const string Key = "Email";
    public string Host { get; set; } = default!;
    public int Port { get; set; }
    public bool EnableSSL { get; set; }
    public int TimeoutMiliseconds { get; set; }
    public string Address { get; set; } = default!;
    public string Password { get; set; } = default!;
}
