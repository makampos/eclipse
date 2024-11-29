using EclipseWorks.Domain.Enum;

namespace EclipseWorks.Domain.Models;

public class TaskHistory : TrackableEntity
{
    public int TaskId { get; private set; }
    public TaskAction? TaskAction { get; private set; } = null;
    public CommentAction? CommentAction { get; private set; } = null;

    public int UserId { get; private set; }

    public string? AffectedProperties { get; private set; } = null;
    public string? Content { get; private set; } = null;

    public DateTime At { get; private set; }

    public virtual User User { get; set; }
    public virtual Task Task { get; set; }

    private TaskHistory(int taskId, int userId, string? affectedProperties = null, string?
            content = null, TaskAction? taskAction = null, CommentAction? commentAction = null)
    {
        TaskId = taskId;
        TaskAction = taskAction;
        CommentAction = commentAction;
        UserId = userId;
        AffectedProperties = affectedProperties;
        Content = content;
        At = DateTime.Now;
    }

    public static TaskHistory Create(int taskId, int userId, string? affectedProperties =
            null,
        string? content = null, TaskAction? taskAction = null, CommentAction? commentAction = null)
    {
        return new TaskHistory(taskId, userId, affectedProperties, content, taskAction, commentAction);
    }

    public static TaskHistory CreteComment(int taskId, int userId, string content)
    {
        return new TaskHistory(
            taskId: taskId,
            commentAction: Enum.CommentAction.Created,
            userId: userId,
            content: content);
    }
}