namespace CityShare.Backend.Application.Core.Dtos.Map;

public class AddressDto
{
    public string DisplayName { get; set; } = default!;
    public PointDto Point { get; set; } = default!;
}
