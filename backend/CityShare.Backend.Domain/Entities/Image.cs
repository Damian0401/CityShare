namespace CityShare.Backend.Domain.Entities;

public class Image
{
    public int Id { get; set; }
    public string Uri { get; set; } = default!;
    public Guid EventId { get; set; }
}
