using CityShare.Backend.Application.Core.Abstractions.Utils;

namespace CityShare.Backend.Infrastructure.Utils;

public class UtcClock : IClock
{
    public DateTime Now => DateTime.UtcNow;
}
