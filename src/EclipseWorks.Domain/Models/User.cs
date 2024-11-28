namespace EclipseWorks.Domain.Models;

public class User : TrackableEntity
{
    public string Username { get; set; }
    public virtual ICollection<ProjectUser> ProjectUsers { get; set; } = [];

    // Navigation property for tasks assigned to the user
    public virtual ICollection<TaskUser> TaskUsers { get; set; } = [];

    private User(string username)
    {
        Username = username;
    }

    public static User Create(string username)
    {
        return new User(username);
    }
}