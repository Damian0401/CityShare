using CityShare.Backend.Application.Core.Dtos.Authentication.Refresh;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Authentication.Commands.Refresh;

public record RefreshCommand(RefreshRequestDto Request, string RefreshToken) 
    : IRequest<Result<RefreshResponseDto>>;
