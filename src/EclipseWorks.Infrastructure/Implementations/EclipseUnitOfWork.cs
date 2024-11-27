using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Interfaces.Repositories;

namespace EclipseWorks.Infrastructure.Implementations;

public class EclipseUnitOfWork(
    ApplicationDbContext applicationDbContext,
    IProjectRepository projectRepository,
    ITaskRepository taskRepository
) : UnitOfWork(applicationDbContext), IEclipseUnitOfWork
{
    public IProjectRepository ProjectRepository { get; init; } = projectRepository;
    public ITaskRepository TaskRepository { get; init; } = taskRepository;
}