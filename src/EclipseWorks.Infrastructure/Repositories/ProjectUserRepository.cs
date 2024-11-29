using EclipseWorks.Domain.Interfaces.Repositories;
using EclipseWorks.Domain.Models;
using EclipseWorks.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorks.Infrastructure.Repositories;

public class ProjectUserRepository
    (ApplicationDbContext applicationDbContext) : Repository<ProjectUser>(applicationDbContext), IProjectUserRepository
{
    public async Task<ICollection<ProjectUser>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
       return applicationDbContext.ProjectUsers.Where(x => x.UserId == userId).ToList();
    }

    public async Task<ICollection<ProjectUser>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
    {
        return await applicationDbContext.ProjectUsers.Where(x => x.ProjectId == projectId)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}