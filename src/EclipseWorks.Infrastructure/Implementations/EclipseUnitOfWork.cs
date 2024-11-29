using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Interfaces.Repositories;

namespace EclipseWorks.Infrastructure.Implementations;

public class EclipseUnitOfWork(
    ApplicationDbContext applicationDbContext,
    IProjectRepository projectRepository,
    ITaskRepository taskRepository,
    IUserRepository userRepository,
    IProjectUserRepository projectUserRepository,
    ITaskHistoryRepository taskHistoryRepository,
    ITaskUserRepository taskUserRepository
) : UnitOfWork(applicationDbContext), IEclipseUnitOfWork
{
    public IProjectRepository ProjectRepository { get; init; } = projectRepository;
    public ITaskRepository TaskRepository { get; init; } = taskRepository;
    public IUserRepository UserRepository { get; init; } = userRepository;
    public IProjectUserRepository ProjectUserRepository { get; init; } = projectUserRepository;
    public ITaskHistoryRepository TaskHistoryRepository { get; init; } = taskHistoryRepository;
    public ITaskUserRepository TaskUserRepository { get; init; } = taskUserRepository;
}