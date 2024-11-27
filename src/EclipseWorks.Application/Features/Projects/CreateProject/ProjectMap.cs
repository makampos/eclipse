using EclipseWorks.Domain.Models;

namespace EclipseWorks.Application.Features.CreateProject;

public static class ProjectMap
{
    public static Project MapToEntity(this CreateProjectCommand command)
    {
       return Project.Create(command.Name, command.Description);
    }

    public static CreateProjectResult MapToCreateProjectResult(this Project project)
    {
        return CreateProjectResult.Create(project.Id, project.Name, project.Description);
    }
}