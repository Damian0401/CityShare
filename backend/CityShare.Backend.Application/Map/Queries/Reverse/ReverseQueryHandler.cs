using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Models.Map.Reverse;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Map.Queries.Reverse;

public class ReverseQueryHandler : IRequestHandler<ReverseQuery, Result<MapReverseResponseModel>>
{
    private readonly INominatimService _nominatimService;
    private readonly IMapper _mapper;

    public ReverseQueryHandler(INominatimService nominatimService, IMapper mapper)
    {
        _nominatimService = nominatimService;
        _mapper = mapper;
    }

    public async Task<Result<MapReverseResponseModel>> Handle(ReverseQuery request, CancellationToken cancellationToken)
    {
        var result = await _nominatimService.ReverseAsync(request.X, request.Y, cancellationToken);

        if (result is null)
        {
            return Result<MapReverseResponseModel>.Failure(Errors.NotFound);
        }

        var mapperResult = _mapper.Map<MapReverseResponseModel>(result);

        return mapperResult;
    }
}
