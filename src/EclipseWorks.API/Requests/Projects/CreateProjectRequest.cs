namespace EclipseWorks.API.Requests.Projects;

public record CreateProjectRequest(string Name, string Description)
{
    public CreateProjectRequest() : this(string.Empty, string.Empty) { }
}