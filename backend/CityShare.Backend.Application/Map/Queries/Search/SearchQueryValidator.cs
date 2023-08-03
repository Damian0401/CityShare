using FluentValidation;

namespace CityShare.Backend.Application.Map.Queries.Search;

public class SearchQueryValidator : AbstractValidator<SearchQuery>
{
    public SearchQueryValidator()
    {
        RuleFor(x => x.Query).NotEmpty();
    }
}
