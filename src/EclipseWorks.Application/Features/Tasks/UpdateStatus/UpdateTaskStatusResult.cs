namespace EclipseWorks.Application.Features.Tasks.UpdateStatus;

public record UpdateTaskStatusResult(string Message)
{
    public UpdateTaskStatusResult() : this(string.Empty) { }
    public static UpdateTaskStatusResult Create(string message) => new UpdateTaskStatusResult(message);
}