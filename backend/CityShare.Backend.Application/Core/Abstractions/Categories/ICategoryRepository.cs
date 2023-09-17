using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Categories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
}
