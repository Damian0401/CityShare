using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Requests;
using CityShare.Backend.Application.Core.Dtos.Requests;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Exceptions;
using CityShare.Backend.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Requests.Queries;

public record GetRequestTypesQuery : IRequest<Result<IEnumerable<RequestTypeDto>>>;

public class GetRequestTypesQueryHandler : IRequestHandler<GetRequestTypesQuery, Result<IEnumerable<RequestTypeDto>>>
{
    private readonly IRequestRepository _requestRepository;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRequestTypesQueryHandler> _logger;

    public GetRequestTypesQueryHandler(IRequestRepository requestRepository, 
        ICacheService cacheService,
        IMapper mapper,
        ILogger<GetRequestTypesQueryHandler> logger)
    {
        _requestRepository = requestRepository;
        _cacheService = cacheService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<RequestTypeDto>>> Handle(GetRequestTypesQuery request, CancellationToken cancellationToken)
    {
        if (_cacheService.TryGet<IEnumerable<RequestTypeDto>>(CacheKeys.RequestTypes, out var cachedDtos))
        {
            var cachedCount = cachedDtos?.Count() ?? throw new InvalidStateException();
            _logger.LogInformation("Returning {@Count} cached request types", cachedCount);
            return Result<IEnumerable<RequestTypeDto>>.Success(cachedDtos);
        }

        _logger.LogInformation("Getting request types from {@Type}", _requestRepository.GetType());
        var types = await _requestRepository.GetTypesAsync(cancellationToken);

        _logger.LogInformation("Mapping request types to DTOs");
        var dtos = _mapper.Map<IEnumerable<RequestTypeDto>>(types);

        _logger.LogInformation("Caching {@Count} request types", dtos.Count());
        var options = new CacheServiceOptions
        {
            Size = dtos.Count()
        };
        _cacheService.Set(CacheKeys.RequestTypes, dtos, options);

        _logger.LogInformation("Returning request types");
        return Result<IEnumerable<RequestTypeDto>>.Success(dtos);
    }
}
