using Microsoft.AspNetCore.Http;

namespace CityShare.Backend.Application.Core.Abstractions.Blobs;

public interface IBlobService
{
    Task<string> UploadFileAsync(
        IFormFile file, 
        string containerName, 
        BlobServiceOptions? options = null, 
        CancellationToken cancellationToken = default);
}

public record BlobServiceOptions
{
    public string? BlobName { get; set; }
    public bool? Overwrite { get; set; }
    public bool? CreateIfNotExists { get; set; }
    public bool? AnonymousRead { get; set; }
};