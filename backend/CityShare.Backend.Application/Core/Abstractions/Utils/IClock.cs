namespace CityShare.Backend.Application.Core.Abstractions.Utils;

public interface IClock
{
    DateTime Now { get; }
}
