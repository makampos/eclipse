using EclipseWorks.Domain.Enum;

namespace EclipseWorks.Application.Features.Tasks.CreateTask;

public record CreateTaskResult(
    int Id,
    string Name,
    string Description,
    int ProjectId,
    PriorityLevel PriorityLevel,
    bool IsCompleted
    )
{
    public static CreateTaskResult Create(int id, string name, string description, int projectId, PriorityLevel priority, bool
            isCompleted)
    {
        return new CreateTaskResult(id, name, description, projectId, priority, isCompleted);
    }
}