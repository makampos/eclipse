using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Interfaces.Repositories;
using EclipseWorks.Infrastructure.Implementations;
using EclipseWorks.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.UnitTests.Handlers;

public abstract class BaseTestHandler<THandler> :  IDisposable, IAsyncDisposable where THandler : class
{
    protected readonly ISampleUnitOfWork SampleUnitOfWork;
    protected readonly ApplicationDbContext _dbContext;
    protected readonly ISampleRepository SampleRepository;
    protected readonly ILogger<THandler> _logger;

    protected BaseTestHandler()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        SampleRepository = new SampleRepository(_dbContext);
        _logger = new Logger<THandler>(new LoggerFactory());
        SampleUnitOfWork = new SampleUnitOfWork(_dbContext, SampleRepository);
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