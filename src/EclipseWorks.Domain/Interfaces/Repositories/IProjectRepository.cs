using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;

namespace EclipseWorks.Domain.Interfaces.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetByIdIncludeNonDeletedTaskAsync(int id,
        Func<IQueryable<Project>, IQueryable<Project>>? includeFunc = null,
        CancellationToken cancellationToken = default);
}