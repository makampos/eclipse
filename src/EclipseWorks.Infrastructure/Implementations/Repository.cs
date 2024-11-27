using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace EclipseWorks.Infrastructure.Implementations;

public class Repository<TEntity>(
    ApplicationDbContext applicationDbContext
    ) : ReadOnlyRepository<TEntity>(applicationDbContext),
    IRepository<TEntity> where TEntity : Entity
{
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is TrackableEntity trackable)
        {
            trackable.Created("user");
        }

        await Set.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is TrackableEntity trackable)
        {
            trackable.Updated("user");
        }

        await Task.Run(() =>
        {
            Set.Update(entity);
        }, cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is TrackableEntity trackable)
        {
            trackable.Deleted("user");

            await Task.Run(() =>
            {

                Set.Update(entity);
            }, cancellationToken);
        }
        else
        {
            await Task.Run(() =>
            {
                Set.Remove(entity);
            }, cancellationToken);
        }
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            if (entity is TrackableEntity trackable)
            {
                trackable.Deleted("user");

                await Task.Run(() =>
                {
                    Set.Update(entity);
                }, cancellationToken);
            }
        }

        await Task.Run(() =>
        {
            Set.RemoveRange(entities);
        }, cancellationToken);
    }
}