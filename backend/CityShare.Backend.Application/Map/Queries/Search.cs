﻿using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Dtos.Map;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Exceptions;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Map.Queries;

public class Search
{
    public record Query(string Phrase) : IRequest<Result<AddressDetailsDto>>;
    
    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Phrase).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Query, Result<AddressDetailsDto>>
    {
        private readonly INominatimService _nominatimService;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private readonly ILogger<Handler> _logger;

        public Handler(
            INominatimService nominatimService, 
            ICacheService cacheService,
            IMapper mapper,
            ILogger<Handler> logger)
        {
            _nominatimService = nominatimService;
            _cacheService = cacheService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<AddressDetailsDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking for {@Query} in cacheService", request);
            if (_cacheService.TryGet<AddressDetailsDto>(request, out var cachedDto))
            {
                _logger.LogInformation("Returning cached dto {@Dto}", cachedDto);
                return cachedDto ?? throw new InvalidStateException();
            }

            _logger.LogInformation("Getting result from {@Service}", _nominatimService.GetType());
            var result = await _nominatimService.SearchByQueryAsync(request.Phrase, cancellationToken);

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
}
