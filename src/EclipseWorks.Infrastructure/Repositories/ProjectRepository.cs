using EclipseWorks.Domain.Interfaces.Repositories;
using EclipseWorks.Domain.Models;
using EclipseWorks.Infrastructure.Implementations;

namespace EclipseWorks.Infrastructure.Repositories;

public class ProjectRepository
    (ApplicationDbContext applicationDbContext) : Repository<Project>(applicationDbContext), IProjectRepository
{

}
