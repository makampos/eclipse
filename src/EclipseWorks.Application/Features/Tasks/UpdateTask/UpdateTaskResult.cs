namespace EclipseWorks.Application.Features.Tasks.UpdateTask;

public record UpdateTaskResult(string Message)
{
    public UpdateTaskResult() : this(string.Empty) { }

    public static UpdateTaskResult Create(string message)
    {
        return new UpdateTaskResult(message);
    }
}