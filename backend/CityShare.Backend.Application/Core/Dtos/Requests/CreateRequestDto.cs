namespace CityShare.Backend.Application.Core.Dtos.Requests;

public class CreateRequestDto
{
    public Guid ImageId { get; set; }
    public string Message { get; set; } = default!;
    public int TypeId { get; set; }
}
