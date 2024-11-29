namespace EclipseWorks.Application.Features.Tasks.CreateTask;
using Task = EclipseWorks.Domain.Models.Task;

public static class TaskMap
{
    public static Task MapToEntity(this CreateTaskCommand command)
    {
        return Task.Create(command.Name, command.Description, command.ProjectId, command.PriorityLevel, command.DueDate);
    }

    public static CreateTaskResult MapToCreateTaskResult(this Task task)
    {
        return CreateTaskResult.Create(task.Id, task.Name, task.Description, task.ProjectId, task.PriorityLevel,
            task.Status, task.DueDate);
    }
}