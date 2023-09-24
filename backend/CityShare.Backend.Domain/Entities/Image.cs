namespace CityShare.Backend.Domain.Entities;

public class Image
{
    public Guid Id { get; set; }
    public string? Uri { get; set; } = default!;
    public Guid EventId { get; set; }
    public bool ShouldBeBlurred { get; set; }
    public bool IsBlurred { get; set; }
}
