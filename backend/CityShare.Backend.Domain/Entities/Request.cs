namespace CityShare.Backend.Domain.Entities;

public class Request
{
    public Guid Id { get; set; }
    public string Message { get; set; } = default!;
    public string AuthorId { get; set; } = default!;
    public Guid EventId { get; set; }
    public int StatusId { get; set; }
    public int TypeId { get; set; }

    public virtual ApplicationUser Author { get; set; } = default!;
    public virtual Event Event { get; set; } = default!;
    public RequestStatus Status { get; set; } = default!;
    public RequestType Type { get; set; } = default!;
}
