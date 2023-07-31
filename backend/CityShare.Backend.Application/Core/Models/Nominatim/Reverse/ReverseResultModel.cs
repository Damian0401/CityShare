﻿namespace CityShare.Backend.Application.Core.Models.Nominatim.Reverse;


public class ReverseResultModel
{
    public int place_id { get; set; }
    public string licence { get; set; } = default!;
    public string osm_type { get; set; } = default!;
    public int osm_id { get; set; }
    public string lat { get; set; } = default!;
    public string lon { get; set; } = default!;
    public string display_name { get; set; } = default!;
    public Address address { get; set; } = default!;
    public string[] boundingbox { get; set; } = default!;
}

