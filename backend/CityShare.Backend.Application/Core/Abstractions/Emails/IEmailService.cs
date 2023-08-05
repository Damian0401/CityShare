using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Emails;

public interface IEmailService
{
    Task SendAsync(Email email);
}
