using CityShare.Backend.Application.Core.Models.Emails;
using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Emails;

public interface IEmailRepository
{
    Task<Guid> CreateAsync(CreateEmailModel model, CancellationToken cancellationToken = default);
    Task<Email?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Email email, CancellationToken cancellationToken = default);
}
