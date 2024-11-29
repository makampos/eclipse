using EclipseWorks.Domain.Enum;
using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.Users.CreateUser;

public record CreateUserCommand(string Username, Role Role) : IRequest<ResultResponse<CreateUserResult>>
{
    public CreateUserCommand() : this(string.Empty, Role.Undefined) { }
    public static CreateUserCommand Create(string username, Role role) => new(username, role);
}