using CityShare.Backend.Application.Core.Contracts.Authentication.Login;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Authentication.Commands.Login;

public record LoginCommand(LoginRequest Request) 
    : IRequest<Result<LoginResponse>>;