using EclipseWorks.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace EclipseWorks.Domain.Interfaces.Abstractions;

public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : Entity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
}