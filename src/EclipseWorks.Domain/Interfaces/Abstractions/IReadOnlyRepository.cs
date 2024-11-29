using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;

namespace EclipseWorks.Domain.Interfaces.Abstractions;

public interface IReadOnlyRepository<TEntity> where TEntity : Entity
{
    Task<ICollection<TEntity>> GetByIdAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> keyComposition, CancellationToken
        cancellationToken =
        default);

    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdIncludeAsync(int id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TEntity>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}