using CityShare.Backend.Domain.Enums;

namespace CityShare.Backend.Domain.Shared;

public class Error : IEquatable<Error>
{
    public static Error None => new Error(ErrorTypes.None, string.Empty);

    public Error(ErrorTypes type, string message)
    {
        Type = type;
        Messages = new() { message };
    }    
    
    public Error(ErrorTypes type, List<string> messages)
    {
        Type = type;
        Messages = messages;
    }

    public ErrorTypes Type { get; }
    public List<string> Messages { get; }

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

        return a.Type == b.Type && a.Messages.SequenceEqual(b.Messages);
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

        hashCode.Add(Type);
        hashCode.Add(Messages);

        return hashCode.ToHashCode();
    }

    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Type == other.Type && Messages.SequenceEqual(other.Messages);
    }
}
