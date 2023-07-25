using CityShare.Backend.Application.Core.Models.Authentication.Register;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Authentication.Commands.Register;

public record RegisterCommand(RegisterRequestModel Request) 
    : IRequest<Result<RegisterResponseModel>>;
