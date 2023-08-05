namespace CityShare.Backend.Domain.Entities;

public class Email
{
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
    public string To { get; set; } = default!;
}
