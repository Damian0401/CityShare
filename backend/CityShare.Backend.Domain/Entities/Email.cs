namespace CityShare.Backend.Domain.Entities;

public class Email
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
    public string Receiver { get; set; } = default!;
    public int TryCount { get; set; }
    public int StatusId { get; set; }
    public DateTime? SendDate { get; set; }
    public int PrirorityId { get; set; }

    public virtual EmailPrirority Prirority { get; set; } = default!;
    public virtual EmailStatus Status { get; set; } = default!;
}
