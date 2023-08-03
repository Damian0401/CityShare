using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Map.Queries.Reverse;

public class ReverseQueryHandler : IRequestHandler<ReverseQuery, Result<ReverseDto>>
{
    private readonly INominatimService _nominatimService;

    public ReverseQueryHandler(INominatimService nominatimService)
    {
        _nominatimService = nominatimService;
    }

    public async Task<Result<ReverseDto>> Handle(ReverseQuery request, CancellationToken cancellationToken)
    {
        var result = await _nominatimService.ReverseAsync(request.X, request.Y);

        if (result is null)
        {
            return Result<ReverseDto>.Failure(Errors.NotFound);
        }

        return result;
    }
}
