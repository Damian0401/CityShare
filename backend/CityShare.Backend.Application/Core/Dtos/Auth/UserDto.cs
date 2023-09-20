namespace CityShare.Backend.Application.Core.Dtos.Auth;

public class UserDto
{
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
    public bool EmailConfirmed { get; set; }
    public IEnumerable<string> Roles { get; set; } = default!;
}
