namespace CityShare.Backend.Application.Core.Dtos.Requests;

public class RequestDto
{
    public Guid Id { get; set; }
    public string Message { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string Author { get; set; } = default!;
    public Guid ImageId { get; set; }
    public Guid EventId { get; set; }
}
