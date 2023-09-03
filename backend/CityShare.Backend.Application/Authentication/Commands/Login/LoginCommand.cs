using CityShare.Backend.Application.Core.Dtos.Authentication.Login;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Authentication.Commands.Login;

public record LoginCommand(LoginRequestDto Request) 
    : IRequest<Result<LoginResponseDto>>;