using EclipseWorks.Domain.Interfaces.Repositories;
using EclipseWorks.Domain.Models;
using EclipseWorks.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorks.Infrastructure.Repositories;

public class ProjectRepository
    (ApplicationDbContext applicationDbContext) : Repository<Project>(applicationDbContext), IProjectRepository
{
    public async Task<Project?> GetByIdIncludeNonDeletedTaskAsync(
        int id, Func<IQueryable<Project>, IQueryable<Project>>? includeFunc = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Project> query = applicationDbContext.Set<Project>();

        if (includeFunc is not null)
        {
            query = includeFunc(query);
        }

        var entity = await query
            .Where(p => p.Id == id)
            .Select(p => new
            {
                Project = p,
                NonDeletedTasks = p.Tasks.Where(t => !t.IsDeleted)
            })
            .FirstOrDefaultAsync(cancellationToken);


        if (entity is not null)
        {
            entity.Project.Tasks = entity.NonDeletedTasks.ToList();
            return entity.Project;
        }

        return null;
    }
}
