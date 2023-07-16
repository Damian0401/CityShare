namespace CityShare.Backend.Domain.Settings;

public class JwtSettings
{
    public const string Key = "Jwt";
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string SecurityKey { get; set; } = default!;
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
}
