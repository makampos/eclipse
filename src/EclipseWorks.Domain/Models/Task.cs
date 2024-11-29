using EclipseWorks.Domain.Enum;
using Microsoft.VisualBasic;

namespace EclipseWorks.Domain.Models;

public class Task : TrackableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public PriorityLevel PriorityLevel { get; init; } // Priority cannot be changed after creation
    public Status Status { get; private set; }
    public int ProjectId { get; private set; }

    public DateOnly DueDate { get; private set; }

    public DateOnly CompletionDate { get; private set; }
    public virtual Project Project { get; private set; }

    // Navigation property to track changes to the task
    public virtual ICollection<TaskHistory> Histories { get; private set; } = [];

    // Navigation property for users assigned to the task
    public virtual ICollection<TaskUser> TaskUsers { get; private set; } = [];

    public Task() { }

   // generate constructor
   private Task(string name, string description, int projectId, PriorityLevel priorityLevel, DateOnly dueDate)
   {
         Name = name;
         Description = description;
         ProjectId = projectId;
         PriorityLevel = priorityLevel;
         DueDate = dueDate;
   }

    public static Task Create(string name, string description, int projectId, PriorityLevel priorityLevel, DateOnly dueDate)
    {
        return new Task(name, description, projectId, priorityLevel, dueDate);
    }

    public void Update(string name, string description, DateOnly dueDate)
    {
        Name = name;
        Description = description;
        DueDate = dueDate;
    }

    public void UpdateStatus(Status status)
    {
        if (status == Status.Completed)
        {
            CompletionDate = DateOnly.FromDateTime(DateTime.Now);
        }

        Status = status;
    }

    public void AddHistory(TaskHistory history)
    {
        Histories.Add(history);
    }

    public void AddTaskUser(TaskUser taskUser)
    {
        TaskUsers.Add(taskUser);
    }

    public string GetDifference(string name, string description, DateOnly dueDate)
    {
        var differences = new List<string>();

        if (Name != name)
        {
            differences.Add(nameof(Name));
        }

        if (Description != description)
        {
            differences.Add(nameof(Description));
        }

        if (DueDate != dueDate)
        {
            differences.Add(nameof(DueDate));
        }

        return string.Join(", ", differences);
    }
}