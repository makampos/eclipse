using EclipseWorks.Domain.Enum;

namespace EclipseWorks.Domain.Models;

public class Task : TrackableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public PriorityLevel PriorityLevel { get; init; } // Priority cannot be changed after creation
    public bool IsCompleted { get; private set; } = false;
    public int ProjectId { get; private set; }
    public virtual Project Project { get; private set; }

    // Navigation property to track changes to the task
    public virtual ICollection<TaskHistory> Histories { get; private set; } = [];

    // Navigation property for users assigned to the task
    public virtual ICollection<TaskUser> TaskUsers { get; private set; } = [];

    public Task() { }

   // generate constructor
   private Task(string name, string description, int projectId, PriorityLevel priorityLevel)
   {
         Name = name;
         Description = description;
         ProjectId = projectId;
         PriorityLevel = priorityLevel;
   }

    public static Task Create(string name, string description, int projectId, PriorityLevel priorityLevel)
    {
        return new Task(name, description, projectId, priorityLevel);
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void UpdateStatus(bool status)
    {
        IsCompleted = status;
    }

    public void AddHistory(TaskHistory history)
    {
        Histories.Add(history);
    }

    public void AddTaskUser(TaskUser taskUser)
    {
        TaskUsers.Add(taskUser);
    }
}