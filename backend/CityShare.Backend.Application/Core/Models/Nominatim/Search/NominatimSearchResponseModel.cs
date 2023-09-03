using System.Text.Json.Serialization;

namespace CityShare.Backend.Application.Core.Models.Nominatim.Search;

public class NominatimSearchResponseModel
{
    public long place_id { get; set; }
    public string licence { get; set; } = default!;
    public string osm_type { get; set; } = default!;
    public long osm_id { get; set; }
    public string[] boundingbox { get; set; } = default!;
    public string lat { get; set; } = default!;
    public string lon { get; set; } = default!;
    public string display_name { get; set; } = default!;
    [JsonPropertyName("class")]
    public string _class { get; set; } = default!;
    public string type { get; set; } = default!;
    public double importance { get; set; }
    public string icon { get; set; } = default!;
}
