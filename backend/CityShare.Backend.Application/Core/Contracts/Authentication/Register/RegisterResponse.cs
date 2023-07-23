using CityShare.Backend.Application.Core.Dtos;
using Microsoft.AspNetCore.Http;

namespace CityShare.Backend.Application.Core.Contracts.Authentication.Register;

public class RegisterResponse
{
    public UserDto User { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;

    public void Deconstruct(out UserDto user, out string refreshToken)
    {
        user = User;
        refreshToken = RefreshToken;
    }
}
