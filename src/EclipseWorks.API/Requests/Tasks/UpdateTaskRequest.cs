using System.Text.Json.Serialization;

namespace EclipseWorks.API.Requests.Tasks;

public record UpdateTaskRequest(
    [property: JsonIgnore] int Id,
    string Name,
    string Description)
{

    public UpdateTaskRequest() : this(0, string.Empty, string.Empty) { }
    public UpdateTaskRequest IncludeId(int id)
    {
       return this with { Id = id };
    }
}