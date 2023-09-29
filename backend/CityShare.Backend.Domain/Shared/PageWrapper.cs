namespace CityShare.Backend.Domain.Shared;

public class PageWrapper<T>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public IEnumerable<T> Content { get; init; } = default!;
}
