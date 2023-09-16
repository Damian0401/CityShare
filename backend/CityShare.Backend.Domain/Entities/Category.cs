namespace CityShare.Backend.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public virtual IEnumerable<Event> Events { get; set; } = default!;
}
