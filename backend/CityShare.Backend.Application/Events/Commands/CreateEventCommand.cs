﻿using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Categories;
using CityShare.Backend.Application.Core.Abstractions.Cities;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Abstractions.Utils;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Application.Core.Dtos.Maps;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Events.Commands;

public record CreateEventCommand(CreateEventDto Request, string UserId)
    : IRequest<Result<Guid>>;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Request.Title)
            .NotEmpty()
            .MaximumLength(256)
            .WithName(x => nameof(x.Request.Title));

        RuleFor(x => x.Request.Description)
            .NotEmpty()
            .MaximumLength(3000)
            .WithName(x => x.Request.Description);

        RuleFor(x => x.Request.CityId)
            .NotEmpty()
            .WithName(x => nameof(x.Request.CityId));

        RuleFor(x => x.Request.Address)
            .NotEmpty()
            .WithName(x => nameof(x.Request.Address));

        RuleFor(x => x.Request.Address.DisplayName)
            .NotEmpty()
            .WithName(x => nameof(x.Request.Address.DisplayName));

        RuleFor(x => x.Request.Address.Point)
            .NotEmpty()
            .WithName(x => nameof(x.Request.Address.Point));

        RuleFor(x => x.Request.Address.Point.X)
            .NotEmpty()
            .WithName(x => nameof(x.Request.Address.Point.X));

        RuleFor(x => x.Request.Address.Point.Y)
            .NotEmpty()
            .WithName(x => nameof(x.Request.Address.Point.Y));

        RuleFor(x => x.Request.CategoryIds)
            .NotEmpty()
            .Must(x => x.Count() > 0).WithMessage(x => $"{nameof(x.Request.CategoryIds)} must contains any ids")
            .Must(x => x.Count() <= Constants.MaxEvenCategoriesNumber).WithMessage(x => $"{nameof(x.Request.CategoryIds)} can contain max {Constants.MaxEvenCategoriesNumber} ids")
            .WithName(x => nameof(x.Request.CategoryIds));

        RuleFor(x => x.Request.StartDate)
            .NotEmpty()
            .WithName(x => nameof(x.Request.StartDate));

        RuleFor(x => x.Request.EndDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.Request.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithName(x => nameof(x.Request.EndDate));
    }
}

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Result<Guid>>
{
    private readonly IEventRepository _eventRepository;
    private readonly ICityRepository _cityRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IClock _clock;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateEventCommandHandler> _logger;

    public CreateEventCommandHandler(
        IEventRepository eventRepository,
        ICityRepository cityRepository,
        ICategoryRepository categoryRepository,
        UserManager<ApplicationUser> userManager,
        IClock clock,
        IMapper mapper,
        ILogger<CreateEventCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _cityRepository = cityRepository;
        _categoryRepository = categoryRepository;
        _userManager = userManager;
        _clock = clock;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Validating request {@Request}", request);
        var errors = await ValidateAsync(request, cancellationToken);

        if (errors.Any())
        {
            _logger.LogError("Request contains invalid data {@Errors}", errors);
            return Result<Guid>.Failure(errors);
        }

        _logger.LogInformation("Creating new Event");
        var eventId = await CreateEventAsync(request);

        return eventId;
    }

    private async Task<Guid> CreateEventAsync(CreateEventCommand request)
    {
        _logger.LogInformation("Mapping DTO to Event");
        var eventToCreate = _mapper.Map<Event>(request.Request);
        eventToCreate.CreatedAt = _clock.Now;
        eventToCreate.AuthorId = request.UserId;

        _logger.LogInformation("Creating Event using {@Type}", _eventRepository.GetType());
        var eventId = await _eventRepository.CreateAsync(eventToCreate);

        _logger.LogInformation("Mapping CategoryIds to EventCategories");
        var eventCategories = request.Request.CategoryIds
            .Select(x => new EventCategory { EventId = eventId, CategoryId = x });

        _logger.LogInformation("Adding EventCategories using {@Type}", _eventRepository.GetType());
        await _eventRepository.AddEventCategoriesAsync(eventCategories);

        return eventId;
    }

    private async Task<IEnumerable<Error>> ValidateAsync(CreateEventCommand request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking if of user with id {@Id} is confirmed", request.UserId);
        var user = await _userManager.FindByIdAsync(request.UserId);
        var isEmailConfirmed = user is not null && user.EmailConfirmed;

        if (!isEmailConfirmed)
        {
            _logger.LogError("Email of user with id {@Id} is not confirmed", request.UserId);
            return Errors.Forbidden;
        }

        _logger.LogInformation("Creating data validation tasks");
        var allTasks = new List<Task<IEnumerable<Error>>>
        {
            CheckIfAddressIsValidAsync(request.Request.CityId, request.Request.Address, cancellationToken),
            CheckIfCategoriesExistsAsync(request.Request.CategoryIds, cancellationToken)
        };

        _logger.LogInformation("Awaiting all data validation tasks");
        var results = await Task.WhenAll(allTasks);

        var errors = results.SelectMany(x => x);

        return errors;
    }

    private async Task<IEnumerable<Error>> CheckIfAddressIsValidAsync(int cityId, AddressDto address, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking if city with id {@Id} exsits", cityId);
        var city = await _cityRepository.GeyByIdWithBoundingBoxAsync(cityId, cancellationToken);

        if (city is null)
        {
            _logger.LogError("City with id {@Id} does not exists", cityId);
            return Errors.CityNotExists(cityId);
        }

        if (!PointIsInsideBoundingBox(address.Point, city.BoundingBox))
        {
            _logger.LogError("Address {@Address} is located outside city {@City}", address, city);
            return Errors.AddressOutsideCity;
        }

        return Enumerable.Empty<Error>();
    }

    private static bool PointIsInsideBoundingBox(PointDto point, BoundingBox boundingBox)
    {
        return point.X > boundingBox.MinX
            && point.X < boundingBox.MaxX
            && point.Y > boundingBox.MinY
            && point.Y < boundingBox.MaxY;
    }

    private async Task<IEnumerable<Error>> CheckIfCategoriesExistsAsync(IEnumerable<int> categoryIds, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all category ids using {@Type}", _categoryRepository.GetType());
        var existingCategoryIds = await _categoryRepository.GetAllIdsAsync(cancellationToken);

        var errors = new List<Error>();

        foreach (var id in categoryIds)
        {
            if (existingCategoryIds.Contains(id))
            {
                continue;
            }

            _logger.LogError("Category with id {@Id} does not exists", id);
            errors.AddRange(Errors.CategoryNotExists(id));
        }

        return errors;
    }
}