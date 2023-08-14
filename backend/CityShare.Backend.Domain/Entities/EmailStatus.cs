namespace CityShare.Backend.Domain.Entities;

public class EmailStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public virtual IEnumerable<Email> Emails { get; set; } = default!;
}
