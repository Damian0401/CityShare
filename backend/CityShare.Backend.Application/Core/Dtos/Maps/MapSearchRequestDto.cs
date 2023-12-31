﻿namespace CityShare.Backend.Application.Core.Dtos.Maps;

public class MapSearchRequestDto
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Postalcode { get; set; }
}
