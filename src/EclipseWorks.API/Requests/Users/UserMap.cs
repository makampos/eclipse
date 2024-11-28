using EclipseWorks.Application.Features.Users.CreateUser;

namespace EclipseWorks.API.Requests.Users;

public static class UserMap
{
    public static CreateUserCommand MapToCommand(this CreateUserRequest request)
    {
        return CreateUserCommand.Create(request.Username);
    }
}