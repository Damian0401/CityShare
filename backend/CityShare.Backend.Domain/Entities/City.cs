namespace CityShare.Backend.Domain.Entities;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int AddressId { get; set; }
    public int BoundingBoxId { get; set; }

    public virtual Address Address { get; set; } = default!;
    public virtual BoundingBox BoundingBox { get; set; } = default!;
    public virtual IEnumerable<Event> Events { get; set; } = default!;
}
