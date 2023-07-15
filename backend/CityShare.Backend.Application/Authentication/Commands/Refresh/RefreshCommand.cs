using CityShare.Backend.Application.Core.Contracts.Authentication.Refresh;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Authentication.Commands.Refresh;

public record RefreshCommand(RefreshRequest Request, string RefreshToken) 
    : IRequest<Result<RefreshResponse>>;
