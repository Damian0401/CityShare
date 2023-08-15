using CityShare.Backend.Application.Core.Dtos;

namespace CityShare.Backend.Application.Core.Models.Map.Reverse;

public class MapReverseResponseModel
{
    public string DisplayName { get; set; } = default!;
    public PointDto Point { get; set; } = default!;
}
