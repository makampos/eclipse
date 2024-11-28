using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.Users.CreateUser;

public record CreateUserCommand(string Username) : IRequest<ResultResponse<CreateUserResult>>
{
    public CreateUserCommand() : this(string.Empty) { }
    public static CreateUserCommand Create(string username) => new(username);
}