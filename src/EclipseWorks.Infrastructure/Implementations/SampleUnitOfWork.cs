using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Interfaces.Repositories;

namespace EclipseWorks.Infrastructure.Implementations;

public class SampleUnitOfWork(
    ApplicationDbContext applicationDbContext,
    ISampleRepository sampleRepository
) : UnitOfWork(applicationDbContext), ISampleUnitOfWork
{
    public ISampleRepository SampleRepository { get; init; } = sampleRepository;
}