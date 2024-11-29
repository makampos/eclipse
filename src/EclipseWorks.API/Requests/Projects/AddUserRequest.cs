using System.Text.Json.Serialization;

namespace EclipseWorks.API.Requests.Projects;

public record AddUserRequest(
    [property: JsonIgnore] int ProjectId,
    int UserId)
{
    public AddUserRequest() : this(0, 0)
    {
    }

    public AddUserRequest Create() => new(ProjectId, UserId);

    public AddUserRequest IncludeProjectId(int projectId)
    {
        return this with { ProjectId = projectId };
    }
}