using FluentValidation;

namespace CityShare.Backend.Application.Map.Queries.Reverse;

public class ReverseQueryValidator : AbstractValidator<ReverseQuery>
{
    public ReverseQueryValidator()
    {
        RuleFor(x => x.X).NotEmpty();
        RuleFor(x => x.Y).NotEmpty();
    }
}
