namespace EclipseWorks.Application.Features.GetProject;
using Task = EclipseWorks.Domain.Models.Task;

public record GetProjectResult(
    int Id,
    string Name,
    string Description,
    ICollection<Task> Tasks)
{
    public static GetProjectResult Create(int id, string name, string description, ICollection<Task> tasks)
    {
        return new GetProjectResult(id, name, description, tasks);
    }
}