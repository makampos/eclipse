namespace EclipseWorks.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public virtual ICollection<ProjectUser> ProjectUsers { get; set; } = [];

    // Navigation property for tasks assigned to the user
    public virtual ICollection<TaskUser> TaskUsers { get; set; } = [];
}