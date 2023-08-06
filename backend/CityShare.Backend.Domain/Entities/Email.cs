namespace CityShare.Backend.Domain.Entities;

public class Email
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
    public string Receiver { get; set; } = default!;
    public int TryCount { get; set; }
    public string Status { get; set; } = default!;
    public DateTime? SendDate { get; set; }
    public int EmailPrirorityId { get; set; }

    public virtual EmailPrirority EmailPrirority { get; set; } = default!;
}
