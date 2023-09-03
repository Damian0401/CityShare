using CityShare.Backend.Application.Core.Dtos.Authentication.ConfirmEmail;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Authentication.Commands.ConfirmEmail;

public record ConfirmEmailCommand(EmailConfirmRequestDto Request) : IRequest<Result>;
