using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Images;

public interface IImageRepository
{
    Task<Guid> CreateAsync(Image image, CancellationToken cancellationToken = default);
    Task SetUriAsync(Guid imageId, string imageUri, CancellationToken cancellationToken = default);
    Task<Image?> GetByIdAsync(Guid imageId, CancellationToken cancellationToken = default);
    Task SetIsBlurredAsync(Guid imageId, bool isBlurred, CancellationToken cancellationToken = default);
}
