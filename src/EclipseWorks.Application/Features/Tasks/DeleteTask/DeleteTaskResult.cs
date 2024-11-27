namespace EclipseWorks.Application.Features.Tasks.DeleteTask;

public record DeleteTaskResult(string Message)
{
    public static DeleteTaskResult Create(string message) => new(message);
}