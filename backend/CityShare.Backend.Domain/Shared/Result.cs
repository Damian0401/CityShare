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
