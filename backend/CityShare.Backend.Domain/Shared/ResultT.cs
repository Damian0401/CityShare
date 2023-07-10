namespace CityShare.Backend.Domain.Shared;

public class Result<TValue> : Result
{
    public Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }
    
    public TValue? Value { get; }

    public static implicit operator Result<TValue>(TValue? value) 
        => new Result<TValue>(value, true, Error.None);

    public static Result<TValue> Success(TValue value) => new(value, true, Error.None);
    public static new Result<TValue> Failure(Error error) => new(default, false, error);
}
