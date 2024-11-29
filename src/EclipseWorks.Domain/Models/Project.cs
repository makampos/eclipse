namespace EclipseWorks.Domain.Models;

public class Project : TrackableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ICollection<Task> Tasks { get; set; } = [];
    public virtual ICollection<ProjectUser> ProjectUsers { get; set; } = [];

    private Project(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Project Create(string name, string description)
    {
        return new Project(name, description);
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void AddTask(Task task)
    {
        Tasks.Add(task);
    }

    public void RemoveTask(Task task)
    {
        Tasks.Remove(task);
    }

    public void RemoveUser(User user)
    {
        var projectUser = ProjectUsers.FirstOrDefault(pu => pu.UserId == user.Id);
        if (projectUser is not null)
        {
            ProjectUsers.Remove(projectUser);
        }
    }
    public void AddProjectUser(ProjectUser projectUser)
    {
        ProjectUsers.Add(projectUser);
    }
}