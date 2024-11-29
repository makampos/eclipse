using System.Text.Json.Serialization;
using EclipseWorks.Domain.Enum;

namespace EclipseWorks.API.Requests.Tasks;

public record UpdateTaskStatusRequest(
    [property: JsonIgnore] int Id,
    Status Status,
    int UserId)
{
    public UpdateTaskStatusRequest() : this(0, Status.None, 0) { }

    public UpdateTaskStatusRequest IncludeId(int id) => this with { Id = id };
    public static UpdateTaskStatusRequest Create(int id, Status status, int userId) => new UpdateTaskStatusRequest(id,
        status, userId);
}