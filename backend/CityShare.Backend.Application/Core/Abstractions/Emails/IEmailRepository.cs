using CityShare.Backend.Application.Core.Models.Emails;
using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Emails;

public interface IEmailRepository
{
    Task<Guid> CreateAsync(CreateEmailModel model);
    Task<Email?> GetByIdAsync(Guid id);
}
