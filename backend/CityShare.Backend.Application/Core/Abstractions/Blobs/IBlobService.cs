using Microsoft.AspNetCore.Http;

namespace CityShare.Backend.Application.Core.Abstractions.Blobs;

public interface IBlobService
{
    Task<string> UploadFileAsync(
        Stream stream,
        string fileName,
        string containerName, 
        BlobServiceUploadOptions? options = null, 
        CancellationToken cancellationToken = default);

    Task<Stream?> ReadFileAsync(
        string fileName,
        string containerName,
        CancellationToken cancellationToken = default);
}

public class BlobServiceUploadOptions
{
    public bool? Overwrite { get; set; }
    public bool? CreateIfNotExists { get; set; }
    public bool? AnonymousRead { get; set; }
};