using System.Text.Json.Serialization;

namespace EclipseWorks.API.Requests.Tasks;

public record UpdateTaskStatusRequest(
    [property: JsonIgnore] int Id,
    bool Status)
{
    public UpdateTaskStatusRequest() : this(0, false) { }

    public UpdateTaskStatusRequest IncludeId(int id) => this with { Id = id };
    public static UpdateTaskStatusRequest Create(int id, bool status) => new UpdateTaskStatusRequest(id, status);
}