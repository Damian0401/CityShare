using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Requests;

public interface IRequestRepository
{
    public Task CreateAsync(Request request, CancellationToken cancellationToken = default);
    public Task<bool> TypeExistsAsync(int typeId, CancellationToken cancellationToken = default);
    public Task<int> GetStatusIdAsync(string statusName, CancellationToken cancellationToken = default);
    public Task<IEnumerable<RequestType>> GetTypesAsync(CancellationToken cancellationToken = default);
    public Task<Request?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    public Task UpdateStatusAsync(Guid requestId, int statusId, CancellationToken cancellationToken = default);
}
