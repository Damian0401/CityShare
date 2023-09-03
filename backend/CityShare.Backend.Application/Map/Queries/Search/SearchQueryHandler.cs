using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Models.Map.Search;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Map.Queries.Search;

public class SearchQueryHandler : IRequestHandler<SearchQuery, Result<MapSearchResponseModel>>
{
    private readonly INominatimService _nominatimService;
    private readonly IMapper _mapper;

    public SearchQueryHandler(INominatimService nominatimService, IMapper mapper)
    {
        _nominatimService = nominatimService;
        _mapper = mapper;
    }

    public async Task<Result<MapSearchResponseModel>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var result = await _nominatimService.SearchByQueryAsync(request.Query, cancellationToken);

        if (result is null)
        {
            return Result<MapSearchResponseModel>.Failure(Errors.NotFound);
        }

        var mapperResult = _mapper.Map<MapSearchResponseModel>(result);

        return mapperResult;
    }
}
