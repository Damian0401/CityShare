using CityShare.Backend.Application.Core.Abstractions.Images;
using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Persistence.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly CityShareDbContext _context;
    private readonly ILogger<ImageRepository> _logger;

    public ImageRepository(
        CityShareDbContext context,
        ILogger<ImageRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(Image image, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding {@Image} to database", image);
        _context.Images.Add(image);
        await _context.SaveChangesAsync(cancellationToken);

        return image.Id;
    }

    public async Task SetImageUriAsync(Guid imageId, string imageUri, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting image with id {@Id} from database", imageId);
        var image = await _context.Images.FirstAsync(x => x.Id.Equals(imageId));

        image.Uri = imageUri;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
