﻿using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Maps;
using CityShare.Backend.Application.Core.Dtos.Maps;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Exceptions;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Maps.Queries;

public record SearchQuery(string Query) : IRequest<Result<AddressDetailsDto>>;

public class SearchQueryValidator : AbstractValidator<SearchQuery>
{
    public SearchQueryValidator()
    {
        RuleFor(x => x.Query).NotEmpty();
    }
}

public class SearchQueryHandler : IRequestHandler<SearchQuery, Result<AddressDetailsDto>>
{
    private readonly IMapService _mapService;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly ILogger<SearchQueryHandler> _logger;

    public SearchQueryHandler(
        IMapService mapService,
        ICacheService cacheService,
        IMapper mapper,
        ILogger<SearchQueryHandler> logger)
    {
        _mapService = mapService;
        _cacheService = cacheService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<AddressDetailsDto>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking for {@Query} in cacheService", request);
        if (_cacheService.TryGet<AddressDetailsDto>(request, out var cachedDto))
        {
            _logger.LogInformation("Returning cached dto {@Dto}", cachedDto);
            return cachedDto ?? throw new InvalidStateException();
        }

        _logger.LogInformation("Getting result from {@Service}", _mapService.GetType());
        var result = await _mapService.SearchByQueryAsync(request.Query, cancellationToken);

        if (result is null)
        {
            _logger.LogWarning("Not found results for {@Query}", request);
            return Result<AddressDetailsDto>.Failure(Errors.NotFound);
        }

        var mapperResult = _mapper.Map<AddressDetailsDto>(result);

        _logger.LogInformation("Caching result {@Result}", result);
        _cacheService.Set(request, mapperResult);

        return mapperResult;
    }
}
