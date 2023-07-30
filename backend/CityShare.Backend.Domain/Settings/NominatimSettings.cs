namespace CityShare.Backend.Domain.Settings;

public class NominatimSettings
{
    public const string Key = "Nominatim";
    public string Url { get; set; } = default!;
}
