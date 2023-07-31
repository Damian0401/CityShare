namespace CityShare.Backend.Application.Core.Dtos;

public class ReverseDto
{
    public string DisplayName { get; set; } = default!;
    public double X { get; set; }
    public double Y { get; set; }
    public string Place { get; set; } = default!;
}
