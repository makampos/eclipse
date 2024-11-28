namespace EclipseWorks.Domain.Models;

public class TaskUser : TrackableEntity
{
    public int UserId { get; private set; }
    public int TaskId { get; private set; }
    public virtual Task Task { get; private set; }
    public virtual User User { get; private set; }

    private TaskUser(int userId, int taskId)
    {
        UserId = userId;
        TaskId = taskId;
    }

    public static TaskUser Create(int userId, int taskId)
    {
        return new TaskUser(userId, taskId);
    }
}