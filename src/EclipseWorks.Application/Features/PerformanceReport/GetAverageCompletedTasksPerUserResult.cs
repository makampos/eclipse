namespace EclipseWorks.Application.Features.PerformanceReport;

public class GetAverageCompletedTasksPerUserResult(int CompletedTasks)
{
    public static GetAverageCompletedTasksPerUserResult Create(int completedTasks)
    {
        return new GetAverageCompletedTasksPerUserResult(completedTasks);
    }
}