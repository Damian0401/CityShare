using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Map.Queries.Search;

public class SearchQueryHandler : IRequestHandler<SearchQuery, Result<SearchDto>>
{
    private readonly INominatimService _nominatimService;

    public SearchQueryHandler(INominatimService nominatimService)
    {
        _nominatimService = nominatimService;
    }

    public async Task<Result<SearchDto>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var result = await _nominatimService.SearchByQueryAsync(request.Query, cancellationToken);

        if (result is null)
        {
            return Result<SearchDto>.Failure(Errors.NotFound);
        }

        return result;
    }
}
