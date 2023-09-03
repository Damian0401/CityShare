using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Dtos.Map;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Map.Queries.Search;

public class SearchQueryHandler : IRequestHandler<SearchQuery, Result<AddressDetailsDto>>
{
    private readonly INominatimService _nominatimService;
    private readonly IMapper _mapper;

    public SearchQueryHandler(INominatimService nominatimService, IMapper mapper)
    {
        _nominatimService = nominatimService;
        _mapper = mapper;
    }

    public async Task<Result<AddressDetailsDto>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var result = await _nominatimService.SearchByQueryAsync(request.Query, cancellationToken);

        if (result is null)
        {
            return Result<AddressDetailsDto>.Failure(Errors.NotFound);
        }

        var mapperResult = _mapper.Map<AddressDetailsDto>(result);

        return mapperResult;
    }
}
