using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Interfaces.Repositories;
using EclipseWorks.Infrastructure.Implementations;
using EclipseWorks.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.UnitTests.Features.Handlers;

public abstract class BaseTestHandler<THandler> :  IDisposable, IAsyncDisposable where THandler : class
{
    protected readonly IEclipseUnitOfWork EclipseUnitOfWork;
    protected readonly ApplicationDbContext _dbContext;
    protected readonly IProjectRepository ProjectRepository;
    protected readonly ITaskRepository TaskRepository;
    protected readonly ILogger<THandler> _logger;

    protected BaseTestHandler()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "EclipseWorks")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        ProjectRepository = new ProjectRepository(_dbContext);
        TaskRepository = new TaskRepository(_dbContext);
        _logger = new Logger<THandler>(new LoggerFactory());
        EclipseUnitOfWork = new EclipseUnitOfWork(_dbContext, ProjectRepository, TaskRepository);
    }

    protected THandler CreateHandler(params object[] parameters)
    {
        return (THandler)Activator.CreateInstance(typeof(THandler), parameters)!;
    }

    public virtual void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    public virtual ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}