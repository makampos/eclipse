using EclipseWorks.Domain.Models;

namespace EclipseWorks.Application.Features.GetProject;

public static class ProjectMap
{
    public static GetProjectResult ToGetProjectResult(this Project project)
    {
        return GetProjectResult.Create(project.Id, project.Name, project.Description, project.Tasks);
    }
}