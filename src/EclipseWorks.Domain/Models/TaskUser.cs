namespace EclipseWorks.Domain.Models;

public class TaskUser
{
    public int UserId { get; set; }
    public virtual User User { get; set; }

    public int TaskId { get; set; }
    public virtual Task Task { get; set; }
}