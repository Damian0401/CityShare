using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Images;

public interface IImageRepository
{
    Task<Guid> CreateAsync(Image image, CancellationToken cancellationToken = default);
    Task SetImageUriAsync(Guid imageId, string imageUri, CancellationToken cancellationToken = default);
}
