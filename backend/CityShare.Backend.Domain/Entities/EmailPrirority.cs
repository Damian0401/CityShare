namespace CityShare.Backend.Domain.Entities;

public class EmailPrirority
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int RetryNumber { get; set; }
    public IEnumerable<Email> Emails { get; set; } = default!;
}
