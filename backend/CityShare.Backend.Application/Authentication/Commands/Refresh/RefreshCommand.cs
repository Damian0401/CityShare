using CityShare.Backend.Application.Core.Models.Authentication.Refresh;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Authentication.Commands.Refresh;

public record RefreshCommand(RefreshRequestModel Request, string RefreshToken) 
    : IRequest<Result<RefreshResponseModel>>;
