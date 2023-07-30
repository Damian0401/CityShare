namespace CityShare.Backend.Application.Core.Dtos;

public class SearchDto
{
    public string DisplayName { get; set; } = default!;
    public double X { get; set; }
    public double Y { get; set; }
}
