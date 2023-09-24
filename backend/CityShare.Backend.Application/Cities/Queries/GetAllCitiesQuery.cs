using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Cities;
using CityShare.Backend.Application.Core.Dtos.Cities;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Exceptions;
using CityShare.Backend.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Cities.Queries;

public record GetAllCitiesQuery : IRequest<Result<IEnumerable<CityDto>>>;

public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, Result<IEnumerable<CityDto>>>
{
    private readonly ICityRepository _cityRepository;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllCitiesQueryHandler> _logger;

    public GetAllCitiesQueryHandler(
        ICityRepository cityRepository,
        ICacheService cacheService,
        IMapper mapper,
        ILogger<GetAllCitiesQueryHandler> logger)
    {
        _cityRepository = cityRepository;
        _cacheService = cacheService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<CityDto>>> Handle(
        GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking for cities in {@Type}", _cacheService.GetType());
        if (_cacheService.TryGet<IEnumerable<CityDto>>(CacheKeys.Cities, out var cachedCities))
        {
            var cachedCount = cachedCities?.Count() ?? throw new InvalidStateException();
            _logger.LogInformation("Returning {@Count} cached cities", cachedCount);
            return Result<IEnumerable<CityDto>>.Success(cachedCities);
        }

        _logger.LogInformation("Getting cities from {@type}", _cityRepository.GetType());
        var cities = await _cityRepository.GetAllWithDetailsAsync(cancellationToken);

        _logger.LogInformation("Mapping cities to DTOs");
        var citiesDto = _mapper.Map<IEnumerable<CityDto>>(cities);

        var count = citiesDto.Count();

        _logger.LogInformation("Caching {@Count} cities", count);
        var options = new CacheServiceOptions
        {
            Size = count
        };
        _cacheService.Set(CacheKeys.Cities, citiesDto, options);

        _logger.LogInformation("Returning cities");
        return Result<IEnumerable<CityDto>>.Success(citiesDto);
    }
}
