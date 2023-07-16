namespace CityShare.Backend.Domain.Settings;

public class CorsSettings
{
    public const string Key = "Cors";
    public const string PolicyName = "Default";
    public string AllowedOrigins { get; set; } = default!;
    public string AllowedMethods { get; set; } = default!;

    public string[] ParsedOrigins => AllowedOrigins.Split(';', StringSplitOptions.TrimEntries);
    public string[] ParsedMethods => AllowedMethods.Split(';', StringSplitOptions.TrimEntries)
        .Select(m => m.ToUpper()).ToArray();
}
