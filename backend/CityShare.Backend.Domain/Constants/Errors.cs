using CityShare.Backend.Domain.Shared;

namespace CityShare.Backend.Domain.Constants;

public static class Errors
{
    public const string InternalServerErrorMessage = "Something went wrong";

    public static IEnumerable<Error> NotFound => new[]
    {
        new Error("NotFound", "Resource not found")
    };

    public static IEnumerable<Error> Forbidden => new[]
    {
        new Error("Forbidden", "User is authorized, but does not have permissions to perform this operation")
    };
    
    public static IEnumerable<Error> InvalidCredentials => new[]
    {
        new Error("InvalidCredentials", "Provided credentials are invalid")
    };

    public static IEnumerable<Error> EmailTaken(string email) => new[]
    {
        new Error("EmailTaken", $"Email {email} is already taken")
    };
    
    public static IEnumerable<Error> InvalidToken => new[]
    {
        new Error("InvalidToken", "Provided token is invalid or expired")
    };    
    
    public static IEnumerable<Error> ForbiddenState => new[]
    {
        new Error("ForbiddenState", "Forbidden state has occurred")
    };

    public static IEnumerable<Error> EmailAlreadyConfirmed => new[]
    {
        new Error("EmailAlreadyConfirmed", "Email is already confirmed")
    };

    public static IEnumerable<Error> CityNotExists(int cityId) => new[]
    {
        new Error("CityNotExists", $"City with id {cityId} does not exists")
    };

    public static IEnumerable<Error> AddressOutsideCity => new[]
    {
        new Error("AddressOutsideCity", $"Provided address is located outside selected city")
    };

    public static IEnumerable<Error> CategoryNotExists(int categoryId) => new[]
    {
        new Error("CategoryNotExists", $"Category with id {categoryId} does not exists")
    };

    public static IEnumerable<Error> MaxImagesNumber => new[]
    {
        new Error("MaxImagesNumber", "Reached maximal number of images for one event")
    };

    public static IEnumerable<Error> ImageSizeLimit => new[]
    {
        new Error("ImageSizeLimit", $"Upload image size limit is {Constants.ImageSizeLimitInMB}MB")
    };
}
