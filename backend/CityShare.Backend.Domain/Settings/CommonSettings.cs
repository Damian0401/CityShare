namespace CityShare.Backend.Domain.Settings;

public class CommonSettings
{
    public const string Key = "Common";
    public string ApplicationName { get; set; } = default!;
    public string ApplicationVersion { get; set; } = default!;
    public string ClientUrl { get; set; } = default!;
}
