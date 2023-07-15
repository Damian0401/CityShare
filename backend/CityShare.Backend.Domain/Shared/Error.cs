using CityShare.Backend.Domain.Constants;

namespace CityShare.Backend.Domain.Shared;

public class Error : IEquatable<Error>
{
    public static Error None => new Error(string.Empty, string.Empty);

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Code == b.Code && a.Message == b.Message;
    }

    public static bool operator !=(Error? a, Error? b) => !(a == b);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        return obj is Error error && Equals(error);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = default;

        hashCode.Add(Code);
        hashCode.Add(Message);

        return hashCode.ToHashCode();
    }

    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code && Message == other.Message;
    }
}
