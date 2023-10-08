using CityShare.Backend.Application.Core.Abstractions.Images;
using CityShare.Backend.Domain.Constants;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using System.Reflection;

namespace CityShare.Backend.Infrastructure.Images;

public class OpenCvSharpService : IImageService
{
    private readonly ILogger<OpenCvSharpService> _logger;
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
    private readonly string _frontalFacePath;
    private readonly string _profileFacePath;

    public OpenCvSharpService(
        ILogger<OpenCvSharpService> logger)
    {
        _logger = logger;

        var basePath = AppDomain.CurrentDomain.BaseDirectory;

        _frontalFacePath = Path.Combine(basePath, CascadeModels.Folder, CascadeModels.Haar.FrontalFace);
        _profileFacePath = Path.Combine(basePath, CascadeModels.Folder, CascadeModels.Haar.ProfileFace);
    }

    public async Task<Stream> BlurFacesAsync(Stream input, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Waiting for thread");
        await _semaphoreSlim.WaitAsync(cancellationToken);

        _logger.LogInformation("Loading classifiers");
        using var frontalHaarCascade = new CascadeClassifier(_frontalFacePath);
        using var profileHaarCascade = new CascadeClassifier(_profileFacePath);

        _logger.LogInformation("Creating {@Type} from input stream", typeof(Mat));
        using var matrix = Mat.FromStream(input, ImreadModes.Color);

        _logger.LogInformation("Bluring faces using frontal classifier");
        BlurFaces(frontalHaarCascade, matrix);

        _logger.LogInformation("Bluring faces using profile classifier");
        BlurFaces(profileHaarCascade, matrix);

        _logger.LogInformation("Flip image");
        Cv2.Flip(matrix, matrix, FlipMode.Y);

        _logger.LogInformation("Bluring faces using profile classifier");
        BlurFaces(profileHaarCascade, matrix);

        _logger.LogInformation("Flip image");
        Cv2.Flip(matrix, matrix, FlipMode.Y);

        _logger.LogInformation("Release semaphore");
        _semaphoreSlim.Release();

        _logger.LogInformation("Creating result");
        var result = matrix.ToMemoryStream();

        return result;
    }

    void BlurFaces(CascadeClassifier cascade, Mat input)
    {
        using var gray = new Mat();

        Cv2.CvtColor(input, gray, ColorConversionCodes.BGR2GRAY);

        var faces = cascade.DetectMultiScale(
            gray, 1.08, 2, HaarDetectionTypes.ScaleImage, new Size(30, 30));

        _logger.LogInformation("Detected {@Number} faces", faces.Length);

        foreach (var face in faces)
        {
            var x1 = face.X;
            var y1 = face.Y;
            var x2 = face.X + face.Width;
            var y2 = face.Y + face.Height;

            var faceImg = new Mat(input,
                    new OpenCvSharp.Range(y1, y2),
                    new OpenCvSharp.Range(x1, x2));

            using var faceBlur = new Mat();
            Cv2.GaussianBlur(faceImg, faceBlur, new Size(23, 23), 30);

            input[new OpenCvSharp.Range(y1, y2), new OpenCvSharp.Range(x1, x2)] = faceBlur;
        }
    }
}
