using Task = EclipseWorks.Domain.Models.Task;

namespace EclipseWorks.Application.Features.CreateProject;

public record CreateProjectResult(
    int Id,
    string Name,
    string Description,
    IReadOnlyCollection<Task> Tasks)
{
    private CreateProjectResult(int id, string name, string description) : this(
        id,
        name,
        description,
        Array.Empty<Task>())
    {
    }

    public static CreateProjectResult Create(int id, string name, string description)
    {
        return new CreateProjectResult(id, name, description);
    }
}