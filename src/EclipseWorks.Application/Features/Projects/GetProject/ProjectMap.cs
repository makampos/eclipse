using EclipseWorks.Application.Features.Tasks.GetTaskResult;
using EclipseWorks.Domain.Models;

namespace EclipseWorks.Application.Features.GetProject;

public static class ProjectMap
{
    public static GetProjectResult ToGetProjectResult(this Project project)
    {
        return GetProjectResult.Create(project.Id, project.Name, project.Description, project.Tasks.Select(x =>
                GetTaskResult.Create(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.PriorityLevel,
                    x.Status,
                    x.DueDate,
                    x.CompletionDate,
                    project.Id
                    )).ToList());
    }
}