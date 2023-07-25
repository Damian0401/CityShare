namespace CityShare.Backend.Application.Core.Models.Authentication.Login;

public class LoginRequestModel
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
