namespace CityShare.Backend.Domain.Exceptions;

/// <summary>
/// Throw when the program is in a state it should never be in
/// </summary>
public class InvalidStateException : Exception
{
    public InvalidStateException()
        : base() { }

    public InvalidStateException(string message)
        : base(message) { }

    public static void ThrowIf(bool condition, string message)
    {
        if (condition)
        {
            throw new InvalidStateException(message);
        }
    }
}
