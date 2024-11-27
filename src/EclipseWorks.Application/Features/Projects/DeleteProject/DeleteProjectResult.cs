namespace EclipseWorks.Application.Features.DeleteProject;

public record DeleteProjectResult(string Message)
{
    public DeleteProjectResult() : this(string.Empty)
    {
    }

    public static DeleteProjectResult Create(string message)
    {
        return new DeleteProjectResult(message);
    }
}