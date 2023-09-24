namespace CityShare.Backend.Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = default!;
    public double X { get; set; }
    public double Y { get; set; }
}
