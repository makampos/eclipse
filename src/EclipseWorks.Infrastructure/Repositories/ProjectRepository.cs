using EclipseWorks.Domain.Interfaces.Repositories;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;
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

    public async Task<PagedResult<Project>> GetProjectsByUserAsync(int pageNumber, int pageSize, int userId, CancellationToken
            cancellationToken
            = default)
    {

        var totalCount = await applicationDbContext.ProjectUsers
            .Where(pu => pu.UserId == userId)
            .CountAsync(cancellationToken: cancellationToken);

        var items = await applicationDbContext.ProjectUsers
            .Where(pu => pu.UserId == userId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => x.Project)
            .ToListAsync(cancellationToken);


        // remove tasks that are deleted from items
        foreach (var project in items)
        {
            project.Tasks = project.Tasks.Where(t => !t.IsDeleted).ToList();
        }


        return PagedResult<Project>.Create(items, totalCount, pageSize, pageNumber);
    }
}
