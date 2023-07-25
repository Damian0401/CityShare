using CityShare.Backend.Application.Core.Dtos;

namespace CityShare.Backend.Application.Core.Models.Authentication.Login;

public class LoginResponseModel
{
    public UserDto User { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;

    public void Deconstruct(out UserDto user, out string refreshToken)
    {
        user = User;
        refreshToken = RefreshToken;
    }
}
