using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.DeleteProject;
using EclipseWorks.Application.Features.GetProject;

namespace EclipseWorks.API.Requests.Projects;

public static class ProjectMap
{
    public static CreateProjectCommand ToCreateProjectCommand(this CreateProjectRequest request)
    {
        return CreateProjectCommand.Create(request.Name, request.Description);
    }

    public static DeleteProjectCommand ToDeleteProjectCommand(this DeleteProjectRequest request)
    {
        return DeleteProjectCommand.Create(request.Id);
    }

    public static GetProjectCommand ToGetProjectCommand(this GetProjectRequest request)
    {
        return GetProjectCommand.Create(request.Id);
    }
}