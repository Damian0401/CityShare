using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Emails.Commands.SendNewEmail;

public record SendNewEmailCommand(Guid EmailId) : IRequest<Result>;
