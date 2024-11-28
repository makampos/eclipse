using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;

namespace EclipseWorks.Domain.Interfaces.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetByIdIncludeNonDeletedTaskAsync(int id,
        Func<IQueryable<Project>, IQueryable<Project>>? includeFunc = null,
        CancellationToken cancellationToken = default);

    Task<PagedResult<Project>> GetProjectsByUserAsync(int pageNumber, int pageSize, int userId,
        CancellationToken cancellationToken = default);
}

