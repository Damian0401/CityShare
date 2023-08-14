using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Domain.Entities;
using Moq;

namespace CityShare.Backend.Tests.Common;

internal class Any
{
    public static string String => It.IsAny<string>();
    public static Guid Guid => It.IsAny<Guid>();
    public static Email Email => It.IsAny<Email>();
    public static ApplicationUser ApplicationUser => It.IsAny<ApplicationUser>();
    public static CancellationToken CancellationToken => It.IsAny<CancellationToken>();
}
