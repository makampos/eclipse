using Microsoft.EntityFrameworkCore;
using EclipseWorks.Domain.Models;

namespace EclipseWorks.Infrastructure.Implementations;

public class RepositoryProperties<TEntity>(
    ApplicationDbContext applicationDbContext
) where TEntity : Entity
{
    protected readonly ApplicationDbContext ApplicationDbContext = applicationDbContext;

    protected DbSet<TEntity> Set => ApplicationDbContext.Set<TEntity>();

    protected IQueryable<TEntity> SetAsTracking
    {
        get
        {
            var query = Set.AsTracking();

            if (typeof(TEntity).IsSubclassOf(typeof(TrackableEntity)))
            {
                query = query.Where(e => !(e as TrackableEntity)!.IsDeleted);
            }

            return query;
        }
    }

    protected IQueryable<TEntity> SetAsNoTracking
    {
        get
        {
            var query = Set.AsNoTracking();

            if (typeof(TEntity).IsSubclassOf(typeof(TrackableEntity)))
            {
                query = query.Where(e => !(e as TrackableEntity)!.IsDeleted);
            }

            return query;
        }
    }
}