using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;

namespace EclipseWorks.Domain.Interfaces.Abstractions;

public interface IReadOnlyRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<TEntity>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}