namespace EclipseWorks.Domain.Models;

public class TaskHistory : TrackableEntity
{
    public int TaskId { get; set; }
    public string ChangeDescription { get; set; }
    public DateTime ChangeDate { get; set; }
    public string UserId { get; set; }
}