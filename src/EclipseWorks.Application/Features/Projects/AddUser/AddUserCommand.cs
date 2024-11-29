using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.AddUser;

public record AddUserCommand(int ProjectId, int UserId) : IRequest<ResultResponse<AddUserResult>>
{
    public AddUserCommand() : this(0, 0)
    {
    }

    public static AddUserCommand Create(int projectId, int userId) => new(projectId, userId);
}