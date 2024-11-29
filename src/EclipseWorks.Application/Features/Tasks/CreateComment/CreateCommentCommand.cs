using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.Tasks.CreateComment;

public record CreateCommentCommand(int TaskId, int UserId, string Content)
    : IRequest<ResultResponse<CreateCommentResult>>
{
    public CreateCommentCommand() : this(0, 0, string.Empty)
    {
    }
    public static CreateCommentCommand Create(int taskId, int userId, string content) => new(taskId, userId, content);
}