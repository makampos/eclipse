using Microsoft.EntityFrameworkCore;
using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;

namespace EclipseWorks.Infrastructure.Implementations;

public class ReadOnlyRepository<TEntity>(
    ApplicationDbContext applicationDbContext
) : RepositoryProperties<TEntity>(applicationDbContext), IReadOnlyRepository<TEntity> where TEntity : Entity
{
    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await SetAsTracking.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<TEntity?> GetByIdIncludeAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity?> query = applicationDbContext.Set<TEntity>();

        if (includeFunc is not null)
        {
            query = includeFunc(query);
        }

        return await query.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<PagedResult<TEntity>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await SetAsNoTracking.CountAsync(cancellationToken);
        var items = await SetAsNoTracking
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return  new PagedResult<TEntity>(items, totalCount, pageSize, pageNumber);
    }
}