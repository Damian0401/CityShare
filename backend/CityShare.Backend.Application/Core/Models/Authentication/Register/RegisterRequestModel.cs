namespace CityShare.Backend.Application.Core.Models.Authentication.Register;

public record RegisterRequestModel(
    string Email,
    string UserName,
    string Password);
