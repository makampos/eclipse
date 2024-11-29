using EclipseWorks.Domain.Enum;

namespace EclipseWorks.Domain.Models;

public class User : TrackableEntity
{
    public string Username { get; set; }

    public Role Role { get; set; }
    public virtual ICollection<ProjectUser> ProjectUsers { get; set; } = [];

    // Navigation property for tasks assigned to the user
    public virtual ICollection<TaskUser> TaskUsers { get; set; } = [];

    private User(string username, Role role = Role.Undefined)
    {
        Username = username;
        Role = role;
    }


    public static User Create(string username, Role role)
    {
        return new User(username, role);
    }
}