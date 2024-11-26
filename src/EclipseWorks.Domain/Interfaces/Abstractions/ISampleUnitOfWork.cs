using EclipseWorks.Domain.Interfaces.Repositories;

namespace EclipseWorks.Domain.Interfaces.Abstractions;

public interface ISampleUnitOfWork : IUnitOfWork
{
    public ISampleRepository SampleRepository { get; init; }
}