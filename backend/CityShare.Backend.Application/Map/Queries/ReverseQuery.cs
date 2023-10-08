using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Maps;
using CityShare.Backend.Application.Core.Dtos.Maps;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Exceptions;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Map.Queries;

public record ReverseQuery(double X, double Y) : IRequest<Result<AddressDto>>;

public class ReverseQueryValidator : AbstractValidator<ReverseQuery>
{
    public ReverseQueryValidator()
    {
        RuleFor(x => x.X).NotEmpty();
        RuleFor(x => x.Y).NotEmpty();
    }
}

public class ReverseQueryHandler : IRequestHandler<ReverseQuery, Result<AddressDto>>
{
    private readonly IMapService _nominatimService;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly ILogger<ReverseQueryHandler> _logger;

    public ReverseQueryHandler(
        IMapService nominatimService,
        ICacheService cacheService,
        IMapper mapper,
        ILogger<ReverseQueryHandler> logger)
    {
        _nominatimService = nominatimService;
        _mapper = mapper;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<Result<AddressDto>> Handle(ReverseQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking for {@Query} in cacheService", request);
        if (_cacheService.TryGet<AddressDto>(request, out var cachedDto))
        {
            _logger.LogInformation("Returning cached dto {@Dto}", cachedDto);
            return cachedDto ?? throw new InvalidStateException();
        }

        _logger.LogInformation("Getting result from {@Service}", _nominatimService.GetType());
        var result = await _nominatimService.ReverseAsync(request.X, request.Y, cancellationToken);

        if (result is null)
        {
            _logger.LogWarning("Not found results for {@Query}", request);
            return Result<AddressDto>.Failure(Errors.NotFound);
        }

        var mapperResult = _mapper.Map<AddressDto>(result);

        _logger.LogInformation("Caching result {@Result}", result);
        _cacheService.Set(request, mapperResult);

        return mapperResult;
    }
}
