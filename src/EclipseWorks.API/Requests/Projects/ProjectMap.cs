using EclipseWorks.Application.Features.AddUser;
using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.DeleteProject;
using EclipseWorks.Application.Features.GetProject;
using EclipseWorks.Application.Features.GetProjectsByUser;

namespace EclipseWorks.API.Requests.Projects;

public static class ProjectMap
{
    public static CreateProjectCommand ToCreateProjectCommand(this CreateProjectRequest request)
    {
        return CreateProjectCommand.Create(request.Name, request.Description, request.UserId);
    }

    public static DeleteProjectCommand ToDeleteProjectCommand(this DeleteProjectRequest request)
    {
        return DeleteProjectCommand.Create(request.Id);
    }

    public static GetProjectCommand ToGetProjectCommand(this GetProjectRequest request)
    {
        return GetProjectCommand.Create(request.Id);
    }

    public static GetProjectsByUserCommand ToGetProjectsByUserCommand(this GetProjectsByUserRequest request)
    {
        return GetProjectsByUserCommand.Create(request.Id, request.PageNumber, request.PageSize);
    }

    public static AddUserCommand ToAddUserCommand(this AddUserRequest request)
    {
        return AddUserCommand.Create(request.ProjectId, request.UserId);
    }
}