using EclipseWorks.Domain.Enum;
using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.Tasks.CreateTask;

public record CreateTaskCommand(
    string Name,
    string Description,
    PriorityLevel PriorityLevel,
    int ProjectId,
    int UserId) : IRequest<ResultResponse<CreateTaskResult>>
{
    public CreateTaskCommand() : this(string.Empty, string.Empty, PriorityLevel.None, 0, 0)
    {

    }
    public static CreateTaskCommand Create(string name, string description, PriorityLevel priorityLevel, int
            projectId, int userId)
    {
        return new CreateTaskCommand(name, description, priorityLevel, projectId, userId);
    }
}