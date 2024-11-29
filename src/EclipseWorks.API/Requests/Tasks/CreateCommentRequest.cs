using System.Text.Json.Serialization;

namespace EclipseWorks.API.Requests.Tasks;

public record CreateCommentRequest(
    [property: JsonIgnore] int TaskId, int UserId, string Content)
{

    public CreateCommentRequest() : this(0, 0, string.Empty) { }

    public CreateCommentRequest IncludeTaskId(int taskId)
    {
        return this with { TaskId = taskId };
    }
}