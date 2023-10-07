namespace CityShare.Backend.Application.Core.Dtos.Maps;

public class AddressDto
{
    public string DisplayName { get; set; } = default!;
    public PointDto Point { get; set; } = default!;
}
