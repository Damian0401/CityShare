using CityShare.Backend.Domain.Entities;
using Moq;

namespace CityShare.Backend.Tests.Common;

internal class Any
{
    public static string String => It.IsAny<string>();
    public static ApplicationUser ApplicationUser => It.IsAny<ApplicationUser>();
}
