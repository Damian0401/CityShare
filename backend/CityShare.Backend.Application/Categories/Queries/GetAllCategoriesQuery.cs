using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Categories;
using CityShare.Backend.Application.Core.Dtos.Categories;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Exceptions;
using CityShare.Backend.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Categories.Queries;

public record GetAllCategoriesQuery : IRequest<Result<IEnumerable<CategoryDto>>>;

public class GetAllCategoriesQueryHandler
    : IRequestHandler<GetAllCategoriesQuery, Result<IEnumerable<CategoryDto>>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllCategoriesQueryHandler> _logger;

    public GetAllCategoriesQueryHandler(
        ICategoryRepository categoryRepository,
        ICacheService cacheService,
        IMapper mapper,
        ILogger<GetAllCategoriesQueryHandler> logger)
    {
        _categoryRepository = categoryRepository;
        _cacheService = cacheService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<CategoryDto>>> Handle(
        GetAllCategoriesQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking for categories in {@Type}", _cacheService.GetType());
        if (_cacheService.TryGet<IEnumerable<CategoryDto>>(CacheKeys.Categories, out var cachedCategories))
        {
            var cachedCount = cachedCategories?.Count() ?? throw new InvalidStateException();
            _logger.LogInformation("Returning {@Count} cached categories", cachedCount);
            return Result<IEnumerable<CategoryDto>>.Success(cachedCategories);
        }

        _logger.LogInformation("Getting categories from {@type}", _categoryRepository.GetType());
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        _logger.LogInformation("Mapping categories to DTOs");
        var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

        var count = categoriesDto.Count();

        _logger.LogInformation("Caching {@Count} categories", count);
        var options = new CacheServiceOptions
        {
            Size = count
        };
        _cacheService.Set(CacheKeys.Categories, categoriesDto, options);

        _logger.LogInformation("Returning categories");
        return Result<IEnumerable<CategoryDto>>.Success(categoriesDto);
    }
}
