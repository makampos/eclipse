using EclipseWorks.Application.Features.Tasks.CreateTask;

namespace EclipseWorks.API.Requests.Tasks;

public static class TaskMap
{
    public static CreateTaskCommand ToCreateTaskCommand(this CreateTaskRequest request)
    {
        return CreateTaskCommand.Create(request.Name, request.Description, request.PriorityLevel, request.ProjectId);
    }
}