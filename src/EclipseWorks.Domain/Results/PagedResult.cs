using System.Text.Json.Serialization;

namespace EclipseWorks.Domain.Results;

public class PagedResult<T>
{
    [JsonConstructor]
    public PagedResult(IReadOnlyList<T>? items, int totalCount, int pageSize, int currentPage)
    {
        Items = items;
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
    }

    public IReadOnlyList<T>? Items { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    // public PagedResult<TDto> ToResult<TEntity>(Func<T, TDto> converter)
    // {
    //     return new PagedResult<TDto>(
    //         Items?.Select(converter).ToList(),
    //         TotalCount,
    //         PageSize,
    //         CurrentPage
    //     );
    // }

    public static PagedResult<T> Create(IReadOnlyList<T>? items, int totalCount, int pageSize, int currentPage)
    {
        return new PagedResult<T>(items, totalCount, pageSize, currentPage);
    }
}