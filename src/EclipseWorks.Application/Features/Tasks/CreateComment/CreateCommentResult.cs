namespace EclipseWorks.Application.Features.Tasks.CreateComment;

public record CreateCommentResult(int Id)
{
    public CreateCommentResult() : this(0)
    {
    }

    public static CreateCommentResult Create(int id) => new(id);
}