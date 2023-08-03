namespace CityShare.Backend.Domain.Settings;

public class CacheSettings
{
    public const string Key = "Cache";
    public int? SlidingExpirationSeconds { get; set; }
    public int? AbsoluteExpirationSeconds { get; set; }
    public int? SizeLimit { get; set; }
}
