using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;

namespace CityShare.Backend.Application.Core.Behaviors;

public class ValidationPipelineBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken _)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var errors = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error is not null)
            .Select(error => new Error(error.ErrorCode, error.ErrorMessage))
            .Distinct()
            .ToList();

        if (errors.Any())
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next();
    }

    private static TResult CreateValidationResult<TResult>(List<Error> errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (Result.Failure(errors) as TResult)!;
        }

        var result = typeof(Result<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments.First())
            .GetMethod(nameof(Result.Failure))!
            .Invoke(null, new object[] { errors });

        return (TResult)result!;
    }
}
