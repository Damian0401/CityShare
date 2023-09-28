using CityShare.Backend.Domain.Enums;

namespace CityShare.Backend.Application.Core.Dtos.Events;

public class EventQueryDto
{
    public string? Query { get; set; }
    public int? CityId { get; set; }
    public string? SkipCategoryIds { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EventSortByOptions? SortBy { get; set; }
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}
