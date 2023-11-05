using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Cities;
using CityShare.Backend.Application.Core.Abstractions.Requests;
using CityShare.Backend.Application.Core.Dtos.Requests;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Requests.Queries;

public record GetPendingRequestsByCityIdQuery(int CityId) : IRequest<Result<GetRequestsDto>>;

public class GetPendingRequestsByCityIdQueryValidator : AbstractValidator<GetPendingRequestsByCityIdQuery>
{
    public GetPendingRequestsByCityIdQueryValidator()
    {
        RuleFor(x => x.CityId)
            .NotEmpty()
            .WithName(x => nameof(x.CityId));
    }
}

public class GetPendingRequestsByCityIdQueryHandler : IRequestHandler<GetPendingRequestsByCityIdQuery, Result<GetRequestsDto>>
{
    private readonly IRequestRepository _requestRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPendingRequestsByCityIdQueryHandler> _logger;

    public GetPendingRequestsByCityIdQueryHandler(
        IRequestRepository requestRepository,
        ICityRepository cityRepository, 
        IMapper mapper,
        ILogger<GetPendingRequestsByCityIdQueryHandler> logger)
    {
        _requestRepository = requestRepository;
        _cityRepository = cityRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<GetRequestsDto>> Handle(GetPendingRequestsByCityIdQuery request, CancellationToken cancellationToken)
    {
        var errors = await ValidateAsync(request.CityId);

        if (errors.Any())
        {
            return Result<GetRequestsDto>.Failure(errors);
        }

        var response = await GetResponse(request, cancellationToken);

        return Result<GetRequestsDto>.Success(response);
    }

    private async Task<GetRequestsDto> GetResponse(GetPendingRequestsByCityIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting pending requests for city with id {@Id} from {@Type}", request.CityId, _requestRepository.GetType());
        var requests = await _requestRepository.GetPendingByCityIdWithDetailsAsync(request.CityId, cancellationToken);

        _logger.LogInformation("Grouping requests by type");
        var groups = requests.GroupBy(x => x.TypeId);

        var map = new Dictionary<int, IEnumerable<RequestDto>>();

        _logger.LogInformation("Mapping requests to DTOs");
        foreach (var group in groups)
        {
            var dtos = _mapper.Map<IEnumerable<RequestDto>>(group.ToList());
            map.Add(group.Key, dtos);
        }

        var result = new GetRequestsDto(map);
        return result;
    }

    private async Task<IEnumerable<Error>> ValidateAsync(int cityId)
    {
        var errors = new List<Error>();

        _logger.LogInformation("Checking if city with id {@Id} exists", cityId);
        var cityExists = await _cityRepository.ExistsAsync(cityId);

        if (!cityExists)
        {
            _logger.LogError("City with id {@Id} does not exist", cityId);
            errors.AddRange(Errors.CityNotExists(cityId));
        }

        return errors;
    }
}