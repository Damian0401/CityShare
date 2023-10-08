using CityShare.Backend.Application.Core.Dtos.Maps;

namespace CityShare.Backend.Application.Core.Dtos.Events;

public class CreateEventDto
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int CityId { get; set; }
    public AddressDto Address { get; set; } = default!;
    public IEnumerable<int> CategoryIds { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
