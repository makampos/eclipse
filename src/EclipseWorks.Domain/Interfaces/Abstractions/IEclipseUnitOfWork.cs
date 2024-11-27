using EclipseWorks.Domain.Interfaces.Repositories;

namespace EclipseWorks.Domain.Interfaces.Abstractions;

public interface IEclipseUnitOfWork : IUnitOfWork
{
    public IProjectRepository ProjectRepository { get; init; }
    public ITaskRepository TaskRepository { get; init; }
}