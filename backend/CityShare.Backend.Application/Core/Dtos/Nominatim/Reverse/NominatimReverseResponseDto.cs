namespace CityShare.Backend.Application.Core.Dtos.Nominatim.Reverse;


public class NominatimReverseResponseDto
{
    public long place_id { get; set; }
    public string licence { get; set; } = default!;
    public string osm_type { get; set; } = default!;
    public long osm_id { get; set; }
    public string lat { get; set; } = default!;
    public string lon { get; set; } = default!;
    public string display_name { get; set; } = default!;
    public string[] boundingbox { get; set; } = default!;
}

