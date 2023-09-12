namespace CityShare.Backend.Domain.Entities;

public class BoundingBox
{
    public int Id { get; set; }
    public double MinX { get; set; }
    public double MaxX { get; set; }
    public double MinY { get; set; }
    public double MaxY { get; set; }
}
