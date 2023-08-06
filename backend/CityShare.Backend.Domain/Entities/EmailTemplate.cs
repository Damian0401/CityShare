namespace CityShare.Backend.Domain.Entities;

public class EmailTemplate
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
}
