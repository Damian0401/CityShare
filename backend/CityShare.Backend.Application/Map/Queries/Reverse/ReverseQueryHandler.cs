using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Models.Map.Reverse;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Map.Queries.Reverse;

public class ReverseQueryHandler : IRequestHandler<ReverseQuery, Result<MapReverseResponseModel>>
{
    private readonly INominatimService _nominatimService;

    public ReverseQueryHandler(INominatimService nominatimService)
    {
        _nominatimService = nominatimService;
    }

    public async Task<Result<MapReverseResponseModel>> Handle(ReverseQuery request, CancellationToken cancellationToken)
    {
        var result = await _nominatimService.ReverseAsync(request.X, request.Y, cancellationToken);

        if (result is null)
        {
            return Result<MapReverseResponseModel>.Failure(Errors.NotFound);
        }

        return result;
    }
}
