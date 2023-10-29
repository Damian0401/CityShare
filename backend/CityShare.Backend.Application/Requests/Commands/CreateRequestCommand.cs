using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Abstractions.Requests;
using CityShare.Backend.Application.Core.Abstractions.Utils;
using CityShare.Backend.Application.Core.Dtos.Requests;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Requests.Commands;

public record CreateRequestCommand(CreateRequestDto Request, string UserId) 
    : IRequest<Result>;

public class CreateRequestCommandValidator : AbstractValidator<CreateRequestCommand>
{
    public CreateRequestCommandValidator()
    {
        RuleFor(x => x.Request.EventId)
            .NotEmpty()
            .WithName(x => nameof(x.Request.EventId));        
        
        RuleFor(x => x.Request.Message)
            .NotEmpty()
            .WithName(x => nameof(x.Request.Message));        
        
        RuleFor(x => x.Request.TypeId)
            .GreaterThanOrEqualTo(0)
            .WithName(x => nameof(x.Request.TypeId));

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}

public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, Result>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly IClock _clock;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<CreateRequestCommandHandler> _logger;

    public CreateRequestCommandHandler(
        IRequestRepository requestRepository,
        IEventRepository eventRepository,
        IMapper mapper,
        IClock clock,
        UserManager<ApplicationUser> userManager,
        ILogger<CreateRequestCommandHandler> logger)
    {
        _requestRepository = requestRepository;
        _eventRepository = eventRepository;
        _userManager = userManager;
        _mapper = mapper;
        _clock = clock;
        _logger = logger;
    }

    public async Task<Result> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var errors = await ValidateAsync(request, cancellationToken);

        if (errors.Any())
        {
            return Result.Failure(errors);
        }

        var newRequest = _mapper.Map<Request>(request.Request);
        newRequest.AuthorId = request.UserId;
        newRequest.StatusId = await _requestRepository.GetStatusIdAsync(RequestStatuses.Pending);
        newRequest.CreatedAt = _clock.Now;

        await _requestRepository.CreateAsync(newRequest, cancellationToken);

        return Result.Success();
    }

    private async Task<IEnumerable<Error>> ValidateAsync(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking if of user with id {@Id} is confirmed", request.UserId);
        var user = await _userManager.FindByIdAsync(request.UserId);
        var isEmailConfirmed = user is not null && user.EmailConfirmed;

        if (!isEmailConfirmed)
        {
            _logger.LogError("Email of user with id {@Id} is not confirmed", request.UserId);
            return Errors.Forbidden;
        }

        _logger.LogInformation("Creating validation tasks");
        var validationTasks = new List<Task<IEnumerable<Error>>>
        {
            CheckIfEventExistsAsync(request.Request.EventId, cancellationToken),
            CheckIfTypeExistsAsync(request.Request.TypeId, cancellationToken)
        };

        var results = await Task.WhenAll(validationTasks);

        var errors = results.SelectMany(x => x);

        return errors;
    }

    private async Task<IEnumerable<Error>> CheckIfTypeExistsAsync(int typeId, CancellationToken cancellationToken)
    {
        var typeExists = await _requestRepository.TypeExistsAsync(typeId, cancellationToken);

        if (!typeExists)
        {
            _logger.LogError("Type with id {@Id} not found", typeId);
            return Errors.RequestTypeNotExists(typeId);
        }

        return Enumerable.Empty<Error>();
    }

    private async Task<IEnumerable<Error>> CheckIfEventExistsAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var eventExists = await _eventRepository.ExistsAsync(eventId, cancellationToken);

        if (!eventExists)
        {
            _logger.LogError("Event with id {@Id} does not exists", eventId);
            return Errors.EventNotExists(eventId);
        }

        return Enumerable.Empty<Error>();
    }
}