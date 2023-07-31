namespace CityShare.Backend.Application.Core.Models.Nominatim;

public class Address
{
    public string? building { get; set; }
    public string? amenity { get; set; }
    public string? house_number { get; set; }
    public string? road { get; set; }
    public string? neighbourhood { get; set; }
    public string? suburb { get; set; }
    public string? county { get; set; }
    public string? city_district { get; set; }
    public string? city { get; set; }
    public string? town { get; set; }
    public string? village { get; set; }
    public string? municipality { get; set; }
    public string state { get; set; } = default!;
    public string ISO31662lvl4 { get; set; } = default!;
    public string postcode { get; set; } = default!;
    public string country { get; set; } = default!;
    public string country_code { get; set; } = default!;
}
