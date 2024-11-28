namespace EclipseWorks.API.Requests.Projects;

public record GetProjectRequest(int Id)
{
    public static GetProjectRequest Create(int id) => new(id);
}