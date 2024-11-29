namespace EclipseWorks.Domain.Models;

public record PerformanceReportResult(int TasksCompleted)
{
    public static PerformanceReportResult Create(int tasksCompleted) => new(tasksCompleted);
}