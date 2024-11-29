using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;

namespace EclipseWorks.Domain.Interfaces.Repositories;

public interface IProjectUserRepository : IRepository<ProjectUser>
{
    Task<ICollection<ProjectUser>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<ICollection<ProjectUser>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
}