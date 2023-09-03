using CityShare.Backend.Application.Core.Dtos.Authentication.Register;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Authentication.Commands.Register;

public record RegisterCommand(RegisterRequestDto Request) 
    : IRequest<Result<RegisterResponseDto>>;
