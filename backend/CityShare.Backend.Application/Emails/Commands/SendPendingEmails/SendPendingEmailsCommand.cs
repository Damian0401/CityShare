using CityShare.Backend.Application.Core.Dtos.Emails.Send;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Emails.Commands.SendPendingEmails;

public record SendPendingEmailsCommand : IRequest<Result<SendPendingEmailsDto>>;
