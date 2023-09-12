namespace CityShare.Backend.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Message { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string AuthorId { get; set; } = default!;
    public Guid EventId { get; set; }
}
