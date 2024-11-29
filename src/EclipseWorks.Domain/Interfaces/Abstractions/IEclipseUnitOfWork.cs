using EclipseWorks.Domain.Interfaces.Repositories;

namespace EclipseWorks.Domain.Interfaces.Abstractions;

public interface IEclipseUnitOfWork : IUnitOfWork
{
    public IProjectRepository ProjectRepository { get; init; }
    public ITaskRepository TaskRepository { get; init; }

    public IUserRepository UserRepository { get; init; }
    public IProjectUserRepository ProjectUserRepository { get; init; }
    public ITaskHistoryRepository TaskHistoryRepository { get; init; }

    public ITaskUserRepository TaskUserRepository { get; init; }
}