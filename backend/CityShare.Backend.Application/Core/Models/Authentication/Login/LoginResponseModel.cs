using CityShare.Backend.Application.Core.Dtos;

namespace CityShare.Backend.Application.Core.Models.Authentication.Login;

public record LoginResponseModel(UserDto User, string RefreshToken)
{
    public void Deconstruct(out UserDto user, out string refreshToken)
    {
        user = User;
        refreshToken = RefreshToken;
    }
}
