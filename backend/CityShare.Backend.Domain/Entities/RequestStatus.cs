namespace CityShare.Backend.Domain.Entities;

public class RequestStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public virtual IEnumerable<Request> Requests { get; set; } = default!;
}
