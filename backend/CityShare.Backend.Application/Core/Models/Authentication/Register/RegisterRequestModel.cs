namespace CityShare.Backend.Application.Core.Models.Authentication.Register;

public class RegisterRequestModel
{
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}
