using EclipseWorks.Domain.Enum;

namespace EclipseWorks.Application.Features.Tasks.CreateTask;

public record CreateTaskResult(
    int Id,
    string Name,
    string Description,
    int ProjectId,
    PriorityLevel PriorityLevel,
    Status Status,
    DateOnly DueDate
    )
{
    public static CreateTaskResult Create(int id, string name, string description, int projectId, PriorityLevel
            priority, Status status, DateOnly dueDate)
    {
        return new CreateTaskResult(id, name, description, projectId, priority, status, dueDate);
    }
}