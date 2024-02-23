using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Common.Models;

public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    
    [UsedImplicitly]
    public int PageNumber { get; }

    [UsedImplicitly]
    public int TotalPages { get; }

    [UsedImplicitly]
    public int TotalCount { get; }

    public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count/(double) pageSize);
        TotalCount = count;
    }

    [UsedImplicitly]
    public bool HasPreviousPage => PageNumber > 1;

    [UsedImplicitly]
    public bool HasNextPage => PageNumber < TotalPages;

    public bool IsEmpty => TotalCount == 0;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}