using EclipseWorks.Domain.Enum;

namespace EclipseWorks.API.Requests.Tasks;

public record CreateTaskRequest(
    string Name,
    string Description,
    PriorityLevel PriorityLevel,
    int ProjectId,
    int UserId,
    DateOnly DueDate)
{
    public CreateTaskRequest() : this(string.Empty, string.Empty, PriorityLevel.None, 0, 0, DateOnly.MinValue)
    {
    }
}