namespace CityShare.Backend.Application.Core.Dtos;

public class SearchDto
{
    public string DisplayName { get; set; } = default!;
    public string Place { get; set; } = default!;
    public double X { get; set; }
    public double Y { get; set; }
    public BoundingBox BoundingBox { get; set; } = default!;
}

public record BoundingBox(double MinX, double MaxX, double MinY, double MaxY);