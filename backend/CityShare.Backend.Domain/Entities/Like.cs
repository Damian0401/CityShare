namespace CityShare.Backend.Domain.Entities;

public class Like
{
    public Guid EventId { get; set; }
    public string AuthorId { get; set; } = default!;
}
