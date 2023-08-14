using CityShare.Backend.Application.Core.Models.Authentication.ConfirmEmail;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Authentication.Commands.ConfirmEmail;

public record ConfirmEmailCommand(EmailConfirmRequestModel Request) : IRequest<Result>;
