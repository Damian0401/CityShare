using CityShare.Backend.Application.Core.Dtos;

namespace CityShare.Backend.Application.Core.Models.Map.Search;

public class MapSearchResponseModel
{
    public string DisplayName { get; set; } = default!;
    public PointDto Point { get; set; } = default!;
    public BoundingBoxDto BoundingBox { get; set; } = default!;
}