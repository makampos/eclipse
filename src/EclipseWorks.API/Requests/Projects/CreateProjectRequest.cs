namespace EclipseWorks.API.Requests.Projects;

public record CreateProjectRequest(string Name, string Description, int UserId)
{
    public CreateProjectRequest() : this(string.Empty, string.Empty, 0) { }
}