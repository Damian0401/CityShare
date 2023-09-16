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

public class Reverse
{
    public record Query(double X, double Y) : IRequest<Result<AddressDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.X).NotEmpty();
            RuleFor(x => x.Y).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Query, Result<AddressDto>>
    {
        private readonly INominatimService _nominatimService;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<Handler> _logger;

        public Handler(
            INominatimService nominatimService, 
            ICacheService cacheService,
            IMapper mapper, 
            ILogger<Handler> logger)
        {
            _nominatimService = nominatimService;
            _mapper = mapper;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Result<AddressDto>> Handle(Query request, CancellationToken cancellationToken)
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
}
