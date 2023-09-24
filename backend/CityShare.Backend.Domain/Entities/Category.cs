namespace CityShare.Backend.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public virtual IEnumerable<EventCategory> EventCategories { get; set; } = default!;
}
