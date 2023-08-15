namespace CityShare.Backend.Application.Core.Models.Nominatim.Search;

public class NominatimSearchParametersModel
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Postalcode { get; set; }
}
