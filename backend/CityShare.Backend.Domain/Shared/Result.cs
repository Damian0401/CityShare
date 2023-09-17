namespace CityShare.Backend.Domain.Shared;

public class Result
{
    public Result(bool isSuccess, IEnumerable<Error>? errors = null)
    {
        if (isSuccess && errors is not null)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && errors is null)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IEnumerable<Error>? Errors { get; }

    public static Result Success() => new(true);
    public static Result Failure(IEnumerable<Error> errors) => new(false, errors);
}

public class Result<TValue> : Result
    where TValue : class
{
    TValue? _value;

    public Result(TValue value, bool isSuccess, IEnumerable<Error>? errors = null)
        : base(isSuccess, errors)
    {
        _value = value;
    }

    public Result(bool isSuccess, IEnumerable<Error>? errors = null)
        : base(isSuccess, errors) { }

    public TValue Value
    {
        get
        {
            if (_value is null || !IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return _value;
        }
    }

    public bool IsEmpty => _value is null;

    public static implicit operator Result<TValue>(TValue value)
        => new(value, true);

    public static Result<TValue> Success(TValue value) => new(value, true);
    public static new Result<TValue> Failure(IEnumerable<Error> errors) => new(false, errors);
}
