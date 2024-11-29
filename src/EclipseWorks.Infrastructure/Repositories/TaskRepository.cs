using EclipseWorks.Domain.Enum;
using EclipseWorks.Domain.Interfaces.Repositories;
using EclipseWorks.Domain.Models;
using EclipseWorks.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using Task = EclipseWorks.Domain.Models.Task;

namespace EclipseWorks.Infrastructure.Repositories;

public class TaskRepository
    (ApplicationDbContext applicationDbContext) : Repository<Task>(applicationDbContext), ITaskRepository
{
    public async Task<PerformanceReportResult> GetAverageCompletedTasksPerUserAsync(int userId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {

        var report = await applicationDbContext.Tasks
            .Where(t => t.Status == Status.Completed &&
                        t.CompletionDate >= startDate &&
                        t.CompletionDate <= endDate &&
                        t.TaskUsers.Any(u => u.UserId == userId)) // Filter by UserId
            .SelectMany(t => t.TaskUsers.Where(u => u.UserId == userId), (task, taskUser) => new
            {
                Task = task, taskUser.UserId
            })
            .GroupBy(x => x.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                CompletedTasksCount = g.Count()
            })
            .ToListAsync(cancellationToken);

        var totalCompletedTasks = report.Sum(r => r.CompletedTasksCount);

        return PerformanceReportResult.Create(totalCompletedTasks);
    }
}