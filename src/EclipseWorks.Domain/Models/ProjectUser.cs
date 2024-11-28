namespace EclipseWorks.Domain.Models;

public class ProjectUser : TrackableEntity
{
    public int UserId { get; private set; }
    public int ProjectId { get; private set; }
    public virtual User User { get; private set; }
    public virtual Project Project { get; private set; }

    public ProjectUser()
    {

    }

    private ProjectUser(int userId, int projectId)
    {
        UserId = userId;
        ProjectId = projectId;
    }
    public static ProjectUser Create(int userId, int projectId)
    {
        return new ProjectUser(userId, projectId);
    }
}