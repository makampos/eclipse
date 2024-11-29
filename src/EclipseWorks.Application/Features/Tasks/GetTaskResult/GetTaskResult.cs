using EclipseWorks.Domain.Enum;

namespace EclipseWorks.Application.Features.Tasks.GetTaskResult;

public record GetTaskResult(
    int Id,
    string Name,
    string Description,
    PriorityLevel PriorityLevel,
    Status Status,
    DateOnly DueDate,
    DateOnly CompletionDate,
    int ProjectId)
{
    public static GetTaskResult Create(int id, string name, string description, PriorityLevel priorityLevel,
        Status status, DateOnly dueDate, DateOnly completionDate, int projectId)
    {
        return new GetTaskResult(id, name, description, priorityLevel, status, dueDate, completionDate, projectId);
    }
}