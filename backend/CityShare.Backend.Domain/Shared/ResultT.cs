namespace CityShare.Backend.Domain.Shared;

public class Result<TValue> : Result
{
    public Result(TValue? value, bool isSuccess, IEnumerable<Error>? errors = null)
        : base(isSuccess, errors)
    {
        Value = value;
    }
    
    public TValue? Value { get; }

    public static implicit operator Result<TValue>(TValue? value) 
        => new Result<TValue>(value, true);

    public static Result<TValue> Success(TValue value) => new(value, true);
    public static new Result<TValue> Failure(IEnumerable<Error> errors) => new(default, false, errors);
}
