using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;
using Task = EclipseWorks.Domain.Models.Task;


namespace EclipseWorks.Domain.Interfaces.Repositories;

public interface ITaskRepository : IRepository<Task>
{
    Task<PerformanceReportResult> GetAverageCompletedTasksPerUserAsync(
        int userId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken);
}