using CityShare.Backend.Application.Core.Dtos.Emails.Create;
using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Emails;

public interface IEmailRepository
{
    Task<Guid> CreateAsync(CreateEmailDto dto, CancellationToken cancellationToken = default);
    Task<Email?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Email email, CancellationToken cancellationToken = default);
    Task<int> GetStatusIdAsync(string statusName, CancellationToken cancellationToken = default);
    Task<IEnumerable<Email>> GetAllWithStatusAsync(string statusName, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmailPriority>> GetAllPrioritiesAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateEmailsAsync(IEnumerable<Email> emails, CancellationToken cancellationToken = default);
}
