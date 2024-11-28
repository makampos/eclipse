using EclipseWorks.Domain.Enum;

namespace EclipseWorks.Domain.Models;

public class TaskHistory : TrackableEntity
{
    public int TaskId { get; private set; }
    public TaskAction TaskAction { get; private set; }
    public int UserId { get; private set; }

    public string? AffectedProperties { get; private set; } = null;

    public TaskHistory()
    {

    }

    private TaskHistory(int taskI, TaskAction taskAction, int userId)
    {
        TaskId = taskI;
        TaskAction = taskAction;
        UserId = userId;
    }

    public static TaskHistory Create(int taskId, TaskAction taskAction, int userId)
    {
        return new TaskHistory(taskId, taskAction, userId);
    }
}