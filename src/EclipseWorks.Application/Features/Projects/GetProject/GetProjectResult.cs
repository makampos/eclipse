using EclipseWorks.Application.Features.Tasks.GetTaskResult;

namespace EclipseWorks.Application.Features.GetProject;

public record GetProjectResult(
    int Id,
    string Name,
    string Description,
    ICollection<GetTaskResult> Tasks)
{
    public static GetProjectResult Create(int id, string name, string description, ICollection<GetTaskResult> tasks)
    {
        return new GetProjectResult(id, name, description, tasks);
    }
}