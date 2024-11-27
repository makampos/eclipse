using EclipseWorks.Application.Features.DeleteProject;

namespace EclipseWorks.API.Requests.Projects;

public record DeleteProjectRequest(int Id)
{
    public DeleteProjectRequest() : this(0)
    {
    }
    public static DeleteProjectRequest Create(int id)
    {
        return new DeleteProjectRequest(id);
    }
}