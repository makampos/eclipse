using EclipseWorks.Domain.Interfaces.Abstractions;

namespace EclipseWorks.Infrastructure.Implementations;

public class UnitOfWork(ApplicationDbContext applicationDbContext) : IUnitOfWork
{
    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await applicationDbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}