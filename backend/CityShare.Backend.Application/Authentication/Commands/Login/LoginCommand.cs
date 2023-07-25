using CityShare.Backend.Application.Core.Models.Authentication.Login;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Authentication.Commands.Login;

public record LoginCommand(LoginRequestModel Request) 
    : IRequest<Result<LoginResponseModel>>;