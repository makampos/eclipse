using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.Application.Features.Tasks.DeleteTask;
using EclipseWorks.Application.Features.Tasks.UpdateTask;

namespace EclipseWorks.API.Requests.Tasks;

public static class TaskMap
{
    public static CreateTaskCommand ToCreateTaskCommand(this CreateTaskRequest request)
    {
        return CreateTaskCommand.Create(request.Name, request.Description, request.PriorityLevel, request.ProjectId);
    }

    public static UpdateTaskCommand ToUpdateTaskCommand(this UpdateTaskRequest request)
    {
        return UpdateTaskCommand.Create(request.Id, request.Name, request.Description);
    }

    public static DeleteTaskCommand ToDeleteTaskCommand(this DeleteTaskRequest request)
    {
        return DeleteTaskCommand.Create(request.Id);
    }
}