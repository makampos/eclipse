using EclipseWorks.Application.Features.PerformanceReport;
using EclipseWorks.Application.Features.Tasks.CreateComment;
using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.Application.Features.Tasks.DeleteTask;
using EclipseWorks.Application.Features.Tasks.UpdateStatus;
using EclipseWorks.Application.Features.Tasks.UpdateTask;

namespace EclipseWorks.API.Requests.Tasks;

public static class TaskMap
{
    public static CreateTaskCommand ToCreateTaskCommand(this CreateTaskRequest request)
    {
        return CreateTaskCommand.Create(request.Name, request.Description, request.PriorityLevel, request.ProjectId,
            request.UserId, request.DueDate);
    }

    public static UpdateTaskCommand ToUpdateTaskCommand(this UpdateTaskRequest request)
    {
        return UpdateTaskCommand.Create(request.Id, request.Name, request.Description, request.UserId, request.DueDate);
    }

    public static DeleteTaskCommand ToDeleteTaskCommand(this DeleteTaskRequest request)
    {
        return DeleteTaskCommand.Create(request.Id);
    }

    public static UpdateTaskStatusCommand ToCompleteTaskCommand(this UpdateTaskStatusRequest request)
    {
        return UpdateTaskStatusCommand.Create(request.Id, request.Status, request.UserId);
    }

    public static CreateCommentCommand ToCreateCommentCommand(this CreateCommentRequest request)
    {
        return CreateCommentCommand.Create(request.TaskId, request.UserId, request.Content);
    }

    public static GetAverageCompletedTasksPerUserCommand ToGetAverageCompletedTasksPerUserCommand(
        this GetAverageCompletedTasksPerUserRequest request)
    {
        return GetAverageCompletedTasksPerUserCommand.Create(request.UserId, request.StartDate, request.EndDate);
    }
}