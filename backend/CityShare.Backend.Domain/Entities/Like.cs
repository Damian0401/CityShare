namespace CityShare.Backend.Domain.Entities;

public class Like
{
    public int Id { get; set; }
    public Guid EventId { get; set; }
    public string AuthorId { get; set; } = default!;
}
