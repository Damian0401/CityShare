namespace CityShare.Backend.Domain.Entities;

public class Email
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
    public string Receiver { get; set; } = default!;
    public int StatusId { get; set; }
    public DateTime? SentDate { get; set; }

    public virtual EmailStatus Status { get; set; } = default!;
}
