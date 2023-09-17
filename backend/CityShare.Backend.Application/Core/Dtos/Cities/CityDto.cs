using CityShare.Backend.Application.Core.Dtos.Map;

namespace CityShare.Backend.Application.Core.Dtos.Cities;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public AddressDetailsDto Address { get; set; } = default!;
}
