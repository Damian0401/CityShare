namespace CityShare.Backend.Application.Core.Contracts.Authentication;

public class RegisterRequest
{
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}
