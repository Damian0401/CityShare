namespace CityShare.Backend.Application.Core.Dtos.Maps;

public class AddressDetailsDto
{
    public string DisplayName { get; set; } = default!;
    public PointDto Point { get; set; } = default!;
    public BoundingBoxDto BoundingBox { get; set; } = default!;
}