namespace CityShare.Backend.Application.Core.Dtos;

public class UserDto
{
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
    public IEnumerable<string> Roles { get; set; } = default!;
}
